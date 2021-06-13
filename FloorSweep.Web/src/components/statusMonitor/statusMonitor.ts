import { writable } from "svelte/store";
import type { RobotCommandMessage } from "../../models/messages/RobotStatusMessage";
import {
  subscribeRobotActionUpdatedAsync,
  subscribeSessionStatusAsync,
} from "../../services/statusService";

export const robotCommandStore = writable({} as RobotCommandMessage);
export const sessionStatusStore = writable("{}");

export async function init(): Promise<void> {
  await subscribeRobotActionUpdatedAsync((message: RobotCommandMessage) =>
    robotCommandStore.set(message)
  );

  await subscribeSessionStatusAsync((message: string) =>
    sessionStatusStore.set(message)
  );
}
