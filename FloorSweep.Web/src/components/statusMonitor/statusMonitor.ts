import { writable } from "svelte/store";
import type { RobotAction } from "../../models/messages/RobotStatusMessage";
import type LocationStatusMessage from "../../models/messages/LocationStatusMessage";
import {
  subscribeRobotActionUpdatedAsync,
  subscribeLocationStatusAsync,
} from "../../services/statusService";

export const robotActionStore = writable({} as RobotAction);
export const locationStatusStore = writable({} as LocationStatusMessage);

export async function init(): Promise<void> {
  await subscribeRobotActionUpdatedAsync((message: RobotAction) =>
    robotActionStore.set(message)
  );

  await subscribeLocationStatusAsync((message: LocationStatusMessage) =>
    locationStatusStore.set(message)
  );
}
