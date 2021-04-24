import { writable } from "svelte/store";
import { subscribe } from "../../services/signalRService";

import type MatrixInitMessage from "../../models/messages/MatrixInitMessage";

export const mapStore = writable([] as MatrixInitMessage[]);

export async function init(mapOnly: boolean): Promise<signalR.HubConnection> {
  const matrixArray = [] as MatrixInitMessage[];
  return await subscribe("MatrixInit", (message: MatrixInitMessage) => {
    if (mapOnly && message.name !== "map") {
      return;
    }
    if (!matrixArray.find((m) => m.name === message.name)) {
      matrixArray[matrixArray.length] = message;
      mapStore.set(matrixArray);
    }
  });
}
