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
  RobotActionType,
} from "../../models/messages/RobotStatusMessage";
import {
  updateStatusAsync,
  subscribeRobotActionUpdatedAsync,
} from "../../services/statusService";
import { Writable, writable } from "svelte/store";

let backgroundImage: ImageData;
let robotImage: HTMLImageElement;
let initHandler: () => void;
let canvasElement: HTMLCanvasElement;
let robotLocation: Point;
let robotRotation: number;
let robotAction: RobotAction = {
  type: RobotActionType.Stopped,
  data: undefined,
};
let started: boolean = false;
const maxDist = 200;
const robotWidth = 20;
const robotHeight = 20;
const halfRobotWidth = robotWidth / 2;
const halfRobotHeight = robotHeight / 2;

export const error: Writable<string> = writable(undefined);

function prepareBackground(
  ctx: CanvasRenderingContext2D,
  img: HTMLImageElement
): ImageData {
  drawImage(ctx, img, 0, 0);
  let imgdta = ctx.createImageData(img.width, img.height);
  return convertBlackAndWhite(imgdta);
}

function redrawScene() {
  const ctx = canvasElement.getContext("2d");
  drawBackground(ctx);
  drawRobot(ctx);
}

export function initializeCanvas(
  canvas: HTMLCanvasElement,
  data: string,
  inited: () => void
) {
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
  canvasElement.addEventListener("click", (e: MouseEvent) => {
    if (!robotLocation) {
      initRobotAsync(getClientCoordinates(e));
    }
  });
  initHandler = inited;
}

function timeOut(timeout: number): Promise<void> {
  return new Promise<void>((resolve, reject) => setTimeout(resolve, timeout));
}
const direction = rotatePoint({ x: 0, y: 1 }, robotRotation);

function getDistanceToObject(): number {
  const rot = d2r(robotRotation);
  let loc = robotLocation;
  let dir = rotatePoint({ x: 0, y: 1 }, robotRotation);
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
    currentAction: robotAction,
    distanceToObject,
  };
}

async function sendRobotStatusUpdateAsync(data: RobotStatusMessage) {
  var result = await updateStatusAsync(data);
  if (result.error) {
    error.set(result.error);
  }
}

async function robotLoopAsync() {
  while (started) {
    switch (robotAction.type) {
      case RobotActionType.Stopped:
        break;
      case RobotActionType.Stop:
        robotAction.data = undefined;
        robotAction.type = RobotActionType.Stopped;
        break;

      case RobotActionType.Turn:
        robotRotation += robotAction.data;
        if (robotRotation > 360) {
          robotRotation -= 360;
        }
        robotAction.type = RobotActionType.Turned;
        robotAction.data = undefined;
        break;
      case RobotActionType.Turned:
        break;
      case RobotActionType.Driving:
        robotLocation = addPoint(robotLocation, direction);
        robotAction.type = RobotActionType.Driving;
        robotAction.data = undefined;
        break;
      case RobotActionType.Drive:
        robotLocation = addPoint(robotLocation, direction);
        break;
    }
    const distance = getDistanceToObject();
    if (distance < 5) {
      robotAction.type = RobotActionType.Stopped;
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

function onRobotUpdated(message: RobotAction) {
  robotAction = message;
}

async function initRobotAsync(p: Point) {
  robotLocation = p;
  robotRotation = 0;
  redrawScene();
  await subscribeRobotActionUpdatedAsync(onRobotUpdated);
  initHandler();
}
