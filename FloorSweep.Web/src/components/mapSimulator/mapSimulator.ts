import {
  addPoint,
  Color,
  convertBlackAndWhite,
  drawImage,
  getClientCoordinates,
  getPixel,
  Point,
  rotatePoint,
  drawLine,
  Line,
} from "../../utils/utils";
import {
  RobotStatusMessage,
  RobotAction,
  RobotCommandType,
  RobotStatusType,
  RobotCommandMessage,
} from "../../models/messages/RobotStatusMessage";
import {
  updateStatusAsync,
  resetStatusAsync,
  subscribeRobotActionUpdatedAsync as subscribeRobotCommandUpdatedAsync,
} from "../../services/statusService";
import { Writable, writable } from "svelte/store";

let backgroundImage: ImageData;
let robotImage: HTMLImageElement;

let canvasElement: HTMLCanvasElement;
let robotLocation: Point;
let robotRotation: number;
let robotUpdateMessage: RobotCommandMessage = undefined;
let currentAction: RobotAction = {
  data: undefined,
  type: RobotStatusType.Stopped,
};
let distanceDebugLine: Line;
let started: boolean = false;
let nextLoopStep: boolean = true;
const loopSpeed = 100;
const maxDist = 200;
const robotWidth = 20;
const robotHeight = 20;
let autoNextStep = false;

export const error: Writable<string> = writable(undefined);
export const location: Writable<Point> = writable(robotLocation);
export const rotation: Writable<number> = writable(robotRotation);
export const autoContinue: Writable<boolean> = writable(false);
autoContinue.subscribe(
  (a) => {
    autoNextStep = a;
    if (a && !nextLoopStep) {
      nextLoopStep = true;
    }
  },
  (a) => {
    autoNextStep = a;
    if (a && !nextLoopStep) {
      nextLoopStep = true;
    }
  }
);

const prepareBackground = (
  ctx: CanvasRenderingContext2D,
  img: HTMLImageElement
): ImageData => {
  const imgdta = drawImage(ctx, img, 0, 0);
  return convertBlackAndWhite(imgdta);
};
function drawBackground(ctx: CanvasRenderingContext2D) {
  ctx.putImageData(backgroundImage, 0, 0);
}

function drawRobot(ctx: CanvasRenderingContext2D) {
  const scalex = robotWidth / robotImage.width;
  const scaley = robotHeight / robotImage.height;
  drawImage(
    ctx,
    robotImage,
    robotLocation.x,
    robotLocation.y,
    robotRotation,
    scalex,
    scaley,
    true
  );
}

function redrawScene() {
  const ctx = canvasElement.getContext("2d");
  drawBackground(ctx);
  drawRobot(ctx);
  if (distanceDebugLine) {
    drawLine(ctx, distanceDebugLine);
  }
}
export const doNextStep = () => (nextLoopStep = true);

export async function initializeCanvasAsync(
  canvas: HTMLCanvasElement,
  data: string,
  inited: () => void
): Promise<void> {
  robotImage = new Image();
  robotImage.src = "/robot.png";

  const bgim = new Image();
  bgim.src = data;
  canvas.width = bgim.width;
  canvas.height = bgim.height;
  const ctx = canvas.getContext("2d");
  backgroundImage = prepareBackground(ctx, bgim);
  drawBackground(ctx);
  canvasElement = canvas;
  canvasElement.addEventListener("click", async (e: MouseEvent) => {
    if (!robotLocation) {
      await initRobotAsync(getClientCoordinates(e), inited);
    }
  });
}

function timeOut(timeout: number): Promise<void> {
  return new Promise<void>((resolve, reject) => setTimeout(resolve, timeout));
}
const direction = (speed?: number) =>
  rotatePoint({ x: 0, y: -(speed ? speed : 1) }, robotRotation);

function getDistanceToObject(debug: boolean = false): number {
  let loc = addPoint(robotLocation, direction(robotHeight / 2));

  const isWhite = (color: Color) => color.r + color.g + color.b === 255 * 3;
  if (!debug) {
    distanceDebugLine = undefined;
  } else {
    distanceDebugLine = { start: loc, end: null };
  }
  let dist = 0;
  for (let i = 1; i < maxDist; i++) {
    const color = getPixel(loc, backgroundImage);
    if (!isWhite(color)) {
      break;
    }
    dist++;
    loc = addPoint(loc, direction());
  }
  if (distanceDebugLine) {
    distanceDebugLine.end = loc;
  }
  return dist;
}

function getRobotStatusData(distanceToObject: number): RobotStatusMessage {
  return {
    currentAction,
    distanceToObject,
    statusDate: new Date(Date.now()),
  };
}

async function sendRobotStatusUpdateAsync(data: RobotStatusMessage) {
  var result = await updateStatusAsync(data);
  error.set(result.error);
}

async function resetRobotStatusUpdateAsync(data: RobotStatusMessage) {
  var result = await resetStatusAsync(data);
  error.set(result.error);
}

function updateStatus(distance: number) {
  if (distance < 5) {
    currentAction.type = RobotStatusType.Stopped;
  } else {
    if (robotUpdateMessage) {
      switch (robotUpdateMessage.type) {
        case RobotCommandType.Drive:
          currentAction.type = RobotStatusType.Driving;
          break;
        case RobotCommandType.Stop:
          currentAction.type = RobotStatusType.Stopped;
          break;
        case RobotCommandType.Turn:
          currentAction.type = RobotStatusType.Turned;
          robotRotation += robotUpdateMessage.data;
          if (robotRotation > 360) {
            robotRotation -= 360;
          }
          rotation.set(robotRotation);
          break;
      }
      currentAction.data = robotUpdateMessage.data;

      robotUpdateMessage = undefined;
    }
  }
}

async function robotLoopAsync() {
  while (started) {
    if (nextLoopStep) {
      if (!autoNextStep) {
        nextLoopStep = false;
      }
      const distance = getDistanceToObject(true);

      updateStatus(distance);
      switch (currentAction.type) {
        case RobotStatusType.Driving:
          robotLocation = addPoint(robotLocation, direction(2));
          location.set(robotLocation);
          break;
      }

      let data = getRobotStatusData(distance);
      redrawScene();
      await sendRobotStatusUpdateAsync(data);
    }
    await timeOut(loopSpeed);
  }
}

export async function startRobotAsync() {
  if (started) {
    return;
  }
  await resetRobotStatusUpdateAsync(
    getRobotStatusData(getDistanceToObject(true))
  );
  started = true;
  await robotLoopAsync();
}
export function stopRobot() {
  if (!started) {
    return;
  }
  started = false;
}

function onRobotUpdated(message: RobotCommandMessage) {
  robotUpdateMessage = message;
}

async function initRobotAsync(p: Point, initHandler: () => void) {
  robotLocation = p;
  location.set(robotLocation);
  robotRotation = 180;
  rotation.set(robotRotation);
  redrawScene();
  await subscribeRobotCommandUpdatedAsync(onRobotUpdated);
  initHandler();
}
