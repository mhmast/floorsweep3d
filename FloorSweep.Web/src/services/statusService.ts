import { writable } from "svelte/store";
import type LocationStatusMessage from "../models/messages/LocationStatusMessage";
import type {
  RobotAction,
  RobotStatusMessage,
} from "../models/messages/RobotStatusMessage";
import { put } from "./baseApi";
import type { Result } from "./fetchSafe";
import { subscribe } from "./signalRService";

export const robotStatusStore = writable({
  currentAction: {},
} as RobotStatusMessage);

export async function subscribeRobotActionUpdatedAsync(
  handler: (message: RobotAction) => void
): Promise<void> {
  await subscribe<RobotAction>("OnRobotAction", handler);
}
export async function subscribeLocationStatusAsync(
  handler: (message: LocationStatusMessage) => void
): Promise<void> {
  await subscribe<LocationStatusMessage>("OnLocationStatusUpdate", handler);
}

export const updateStatusAsync = async (
  status: RobotStatusMessage
): Promise<Result<void>> => {
  robotStatusStore.set(status);
  return await put<void>("/robotStatus", status);
};
