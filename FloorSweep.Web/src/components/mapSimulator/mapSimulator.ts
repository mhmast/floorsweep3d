import { getClientCoordinates } from "../../utils/utils";
import type Point from "../../models/point";
import { RobotActionType } from "../../models/messages/RobotStatusMessage";
import { updateStatusAsync } from "../../services/statusService";

let image: HTMLImageElement, robotImage: HTMLImageElement;
let initHandler: () => void;
let canvasElement: HTMLCanvasElement;
let robotLocation: Point;
let robotRotation: number;

export function initializeCanvas(
  canvas: HTMLCanvasElement,
  data: string,
  inited: () => void
) {
  image = new Image();
  robotImage = new Image();
  robotImage.src = "/robot.png";
  image.onload = () => {
    canvas.width = image.width;
    canvas.height = image.height;
    const ctx = canvas.getContext("2d");
    ctx.drawImage(image, 0, 0);
  };
  image.src = data;
  canvasElement = canvas;
  canvasElement.addEventListener("click", (e: MouseEvent) =>
    initRobot(getClientCoordinates(e))
  );
  initHandler = inited;
}

function drawImage(
  context: CanvasRenderingContext2D,
  image: CanvasImageSource,
  x: number,
  y: number,
  rotation?: number
) {
  context.translate(x, y); //translate to center of shape
  context.rotate((Math.PI / 180) * rotation); //rotate 25 degrees.
  context.translate(-x, -y);
  context.drawImage(image, x, y);
}

function drawRobot() {
  const ctx = canvasElement.getContext("2d");
  drawImage(ctx, image, 0, 0);
  let halfx = robotImage.width / 2;
  let halfy = robotImage.height / 2;

  ctx.save();
  ctx.setTransform({});
  drawImage(ctx, robotImage, robotLocation.x - halfx, robotLocation.y - halfy);
  ctx.restore();
}

async function initRobot(p: Point) {
  robotLocation = p;
  robotDirection = { x: 0, y: 1 };
  initHandler();
  drawRobot();
  await updateStatusAsync({
    currentAction: {
      data: undefined,
      type: RobotActionType.Stopped,
    },
    distanceToObject: 0,
  });
}
