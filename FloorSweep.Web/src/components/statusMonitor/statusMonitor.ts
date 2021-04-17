import * as signalR from '@microsoft/signalr';
import { Writable, writable } from 'svelte/store';
import ConfigStore from '../../store/configStore';
import type RobotStatusMessage from '../../models/messages/RobotStatusMessage';
import TokenStore from '../../store/tokenStore';

export const statusStore = writable({currentAction:{}} as RobotStatusMessage);
let connection:signalR.HubConnection;


export async function init():Promise<signalR.HubConnection> {
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
  
      connection.on('OnStatusUpdate', (message:RobotStatusMessage) => {
        
          statusStore.set(message);
      });
    
    }
    
    if(connection.state !== signalR.HubConnectionState.Connected)
    {
      await connection.start();
    } 
    return connection;
}
