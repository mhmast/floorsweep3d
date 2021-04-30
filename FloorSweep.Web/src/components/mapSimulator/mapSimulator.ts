import {
  addPoint,
  Color,
  convertBlackAndWhite,
  d2r,
  drawImage,
  getClientCoordinates,
  getPixel,
  rotatePoint,
} from "../../utils/utils";
import type Point from "../../models/point";
import {
  RobotStatusMessage,
  RobotAction,
  RobotCommandType,
  RobotStatusType,
  RobotCommandMessage,
} from "../../models/messages/RobotStatusMessage";
import {
  updateStatusAsync,
  subscribeRobotActionUpdatedAsync,
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

let started: boolean = false;
const maxDist = 200;
const robotWidth = 20;
const robotHeight = 20;
const halfRobotWidth = robotWidth / 2;
const halfRobotHeight = robotHeight / 2;

export const error: Writable<string> = writable(undefined);

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
    robotLocation.x - halfRobotWidth,
    robotLocation.y - halfRobotHeight,
    robotRotation,
    scalex,
    scaley
  );
}

function redrawScene() {
  const ctx = canvasElement.getContext("2d");
  drawBackground(ctx);
  drawRobot(ctx);
}

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
const direction = rotatePoint({ x: 0, y: 1 }, robotRotation);

function getDistanceToObject(): number {
  const rot = d2r(robotRotation);
  let loc = robotLocation;
  let dir = rotatePoint({ x: 0, y: 1 }, rot);
  const isWhite = (color: Color) => color.r + color.g + color.b === 255 * 3;
  let dist = 0;
  for (let i = 1; i < maxDist; i++) {
    const color = getPixel(loc, backgroundImage);
    if (!isWhite(color)) {
      break;
    }
    dist++;
    loc = addPoint(loc, dir);
  }
  return dist;
}

function getRobotStatusData(distanceToObject: number): RobotStatusMessage {
  return {
    currentAction,
    distanceToObject,
  };
}

async function sendRobotStatusUpdateAsync(data: RobotStatusMessage) {
  var result = await updateStatusAsync(data);
  error.set(result.error);
}

function updateStatus() {
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
        break;
    }
    currentAction.data = robotUpdateMessage.data;
    robotUpdateMessage = undefined;
  }
}

async function robotLoopAsync() {
  while (started) {
    updateStatus();
    switch (currentAction.type) {
      case RobotStatusType.Driving:
        robotLocation = addPoint(robotLocation, direction);
        break;
    }
    const distance = getDistanceToObject();
    if (distance < 5) {
      currentAction.type = RobotStatusType.Stopped;
    }
    const data = getRobotStatusData(distance);
    redrawScene();
    await sendRobotStatusUpdateAsync(data);
    await timeOut(1000);
  }
}

export async function startRobotAsync() {
  if (started) {
    return;
  }
  await sendRobotStatusUpdateAsync(getRobotStatusData(getDistanceToObject()));
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
  robotRotation = 0;
  redrawScene();
  await subscribeRobotActionUpdatedAsync(onRobotUpdated);
  initHandler();
}
