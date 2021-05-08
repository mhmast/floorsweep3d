import { writable } from "svelte/store";
import type { RobotCommandMessage } from "../../models/messages/RobotStatusMessage";
import type { LocationStatusMessage } from "../../models/messages/LocationStatusMessage";
import type { DiagnosticStatusMessage } from "../../models/messages/DiagnosticStatusMessage";
import {
  subscribeRobotActionUpdatedAsync,
  subscribeLocationStatusAsync,
  subscribeDiagnosticStatusAsync,
} from "../../services/statusService";

export const robotCommandStore = writable({} as RobotCommandMessage);
export const diagnosticStatusStore = writable({} as DiagnosticStatusMessage);
export const locationStatusStore = writable({} as LocationStatusMessage);

export async function init(): Promise<void> {
  await subscribeRobotActionUpdatedAsync((message: RobotCommandMessage) =>
    robotCommandStore.set(message)
  );

  await subscribeLocationStatusAsync((message: LocationStatusMessage) =>
    locationStatusStore.set(message)
  );

  await subscribeDiagnosticStatusAsync((message: DiagnosticStatusMessage) =>
    diagnosticStatusStore.set(message)
  );
}
