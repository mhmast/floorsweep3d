import * as signalR from '@microsoft/signalr';
import { Writable, writable } from 'svelte/store';
import ConfigStore from '../../store/configStore';
import type MatrixInitMessage from '../../models/messages/MatrixInit';
import TokenStore from '../../store/tokenStore';

export const matrixStore = writable([] as MatrixInitMessage[]);
let connection:signalR.HubConnection;

export async function init():Promise<Writable<MatrixInitMessage[]>> {
  const matrixArray = [] as MatrixInitMessage[];
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
      .configureLogging(signalR.LogLevel.Error)
      .withAutomaticReconnect()
      .build();
  
      connection.on('OnMatrixInit', (message:MatrixInitMessage) => {
        console.log(message);
        matrixArray[matrixArray.length] = message;
        matrixStore.set(matrixArray);
      });
    
    }
    
    if(connection.state !== signalR.HubConnectionState.Connected)
    {
      console.log(" startsrat")
      await connection.start();
    } 
    return matrixStore;
}
