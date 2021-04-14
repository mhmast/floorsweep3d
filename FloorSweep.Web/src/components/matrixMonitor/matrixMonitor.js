import * as signalR from '@microsoft/signalr';
import { writable } from 'svelte/store';
import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
export const matrixStore = writable([]);
let connection;
export async function init() {
    const matrixArray = [];
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
        connection.on('OnMatrixInit', (message) => {
            console.log(message);
            matrixArray[matrixArray.length] = message;
            matrixStore.set(matrixArray);
        });
    }
    if (connection.state !== signalR.HubConnectionState.Connected) {
        await connection.start();
    }
    return connection;
}
//# sourceMappingURL=matrixMonitor.js.map