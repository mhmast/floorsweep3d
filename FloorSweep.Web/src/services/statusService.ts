import { writable } from "svelte/store";
import type LocationStatusMessage from "../models/messages/LocationStatusMessage";
import type {
  RobotAction,
  RobotCommandMessage,
  RobotStatusMessage,
} from "../models/messages/RobotStatusMessage";
import { put } from "./baseApi";
import type { Result } from "./fetchSafe";
import { subscribe } from "./signalRService";

export const robotStatusStore = writable({
  currentAction: {},
} as RobotStatusMessage);

export async function subscribeRobotActionUpdatedAsync(
  handler: (message: RobotCommandMessage) => void
): Promise<void> {
  await subscribe<RobotCommandMessage>("OnRobotStatusUpdated", handler);
}
export async function subscribeLocationStatusAsync(
  handler: (message: LocationStatusMessage) => void
): Promise<void> {
  await subscribe<LocationStatusMessage>("OnLocationStatusUpdated", handler);
}

export const updateStatusAsync = async (
  status: RobotStatusMessage
): Promise<Result<void>> => {
  robotStatusStore.set(status);
  return await put<void>("/robot/status", status);
};
