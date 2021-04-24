import type Point from "../models/point";

export function getClientCoordinates(e: MouseEvent): Point {
  return { x: e.offsetX, y: e.offsetY };
}
