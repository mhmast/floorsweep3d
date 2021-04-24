import * as signalR from "@microsoft/signalr";
import TokenStore from "../store/tokenStore";
import ConfigStore from "../store/configStore";
import { loginRedirect } from "./authenticationService";

let connection: signalR.HubConnection;

export async function subscribe<T>(
  eventName: string,
  handler: (message: T) => void
): Promise<signalR.HubConnection> {
  const configStore = ConfigStore.create();
  const config = await configStore.await();
  const tokenStore = TokenStore.create();
  const tokenData = await tokenStore.await();
  if (!connection) {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${config.apiBaseUrl}/hubs/monitor`, {
        accessTokenFactory: () => {
          return tokenData.token;
        },
      })
      .configureLogging(signalR.LogLevel.Error)
      .withAutomaticReconnect()
      .build();
  }
  connection.on(eventName, handler);

  if (connection.state !== signalR.HubConnectionState.Connected) {
    await connection.start().catch((reason) => {
      if (reason.statusCode === 401 || reason.statusCode === 403) {
        loginRedirect(config);
      }
    });
  }
  return connection;
}
