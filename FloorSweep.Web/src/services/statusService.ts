import type LocationStatusMessage from "../models/messages/LocationStatusMessage";
import type RobotStatusMessage from "../models/messages/RobotStatusMessage";
import { put } from "./baseApi";
import { subscribe } from "./signalRService";

export async function subscribeRobotStatusAsync(
  handler: (message: RobotStatusMessage) => void
): Promise<void> {
  await subscribe<RobotStatusMessage>("OnRobotStatusUpdate", handler);
}
export async function subscribeLocationStatusAsync(
  handler: (message: LocationStatusMessage) => void
): Promise<void> {
  await subscribe<LocationStatusMessage>("OnLocationStatusUpdate", handler);
}

export async function updateStatusAsync(status: RobotStatusMessage) {
  await put("/robotStatus", status);
}
