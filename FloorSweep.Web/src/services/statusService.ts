import { writable } from "svelte/store";
import type { DiagnosticStatusMessage } from "../models/messages/DiagnosticStatusMessage";
import type { LocationStatusMessage } from "../models/messages/LocationStatusMessage";
import type {
  RobotCommandMessage,
  RobotStatusMessage,
} from "../models/messages/RobotStatusMessage";
import { put, post } from "./baseApi";
import type { Result } from "./fetchSafe";
import { subscribe } from "./signalRService";

export const robotStatusStore = writable({
  currentAction: {},
} as RobotStatusMessage);

export async function subscribeRobotActionUpdatedAsync(
  handler: (message: RobotCommandMessage) => void
): Promise<void> {
  await subscribe<RobotCommandMessage>("OnRobotCommand", (m) => {
    handler(m);
  });
}
export async function subscribeLocationStatusAsync(
  handler: (message: LocationStatusMessage) => void
): Promise<void> {
  await subscribe<LocationStatusMessage>("OnLocationStatusUpdated", (m) => {
    handler(m);
  });
}
export async function subscribeDiagnosticStatusAsync(
  handler: (message: DiagnosticStatusMessage) => void
): Promise<void> {
  await subscribe<DiagnosticStatusMessage>("OnDiagnosticStatusUpdated", (m) => {
    handler(m);
  });
}

export const updateStatusAsync = async (
  status: RobotStatusMessage
): Promise<Result<void>> => {
  robotStatusStore.set(status);
  return await put<void>("/robot/status", status);
};

export const resetStatusAsync = async (
  status: RobotStatusMessage
): Promise<Result<void>> => {
  robotStatusStore.set(status);
  return await post<void>("/robot/status", status);
};
