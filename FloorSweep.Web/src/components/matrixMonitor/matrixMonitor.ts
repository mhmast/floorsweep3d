import * as signalR from '@microsoft/signalr';
import { Writable, writable } from 'svelte/store';
import ConfigStore from '../../store/configStore';
import type MatrixInitMessage from '../../models/messages/MatrixInitMessage';
import TokenStore from '../../store/tokenStore';

export const matrixStore = writable([] as MatrixInitMessage[]);
let connection:signalR.HubConnection;


export async function init():Promise<signalR.HubConnection> {
  const matrixArray = [] as MatrixInitMessage[];
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
  
      connection.on('OnMatrixInit', (message:MatrixInitMessage) => {
        if(!matrixArray.find(m=>m.name === message.name))
        {
          matrixArray[matrixArray.length] = message;
          matrixStore.set(matrixArray);
        }
      });
    
    }
    
    if(connection.state !== signalR.HubConnectionState.Connected)
    {
      await connection.start();
    } 
    return connection;
}
