import { writable } from "svelte/store";
import type RobotStatusMessage from "../../models/messages/RobotStatusMessage";
import type LocationStatusMessage from "../../models/messages/LocationStatusMessage";
import {
  subscribeRobotStatusAsync,
  subscribeLocationStatusAsync,
} from "../../services/statusService";

export const robotStatusStore = writable({
  currentAction: {},
} as RobotStatusMessage);
export const locationStatusStore = writable({} as LocationStatusMessage);

export async function init(): Promise<void> {
  await subscribeRobotStatusAsync((message: RobotStatusMessage) =>
    robotStatusStore.set(message)
  );

  await subscribeLocationStatusAsync((message: LocationStatusMessage) =>
    locationStatusStore.set(message)
  );
}
