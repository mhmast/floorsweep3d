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
    if (!connection) {
        connection = new signalR.HubConnectionBuilder()
            .withUrl(`${config.apiBaseUrl}/hubs/monitor`, {
            accessTokenFactory: async () => {
                const tokenStore = TokenStore.create();
                const tokenData = await tokenStore.await();
                return tokenData.token;
            },
        })
            .configureLogging(signalR.LogLevel.Trace)
            .withAutomaticReconnect()
            .build();
    }
    connection.on('OnMatrixInit', (message) => {
        console.log(message);
        matrixArray[matrixArray.length] = message;
        matrixStore.set(matrixArray);
    });
    await connection.start();
    return matrixStore;
}
//# sourceMappingURL=matrixMonitor.js.map