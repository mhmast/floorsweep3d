import { writable } from "svelte/store";
import type { RobotCommandMessage } from "../../models/messages/RobotStatusMessage";
import {
  subscribeRobotActionUpdatedAsync,
  subscribeSessionUpdatedAsync,
} from "../../services/statusService";

export const robotCommandStore = writable({} as RobotCommandMessage);
export const sessionStatusStore = writable({} as object);

export async function init(): Promise<void> {
  await subscribeRobotActionUpdatedAsync((message: RobotCommandMessage) =>
    robotCommandStore.set(message)
  );

  await subscribeSessionUpdatedAsync((message: object) =>
    sessionStatusStore.set(message)
  );
}
