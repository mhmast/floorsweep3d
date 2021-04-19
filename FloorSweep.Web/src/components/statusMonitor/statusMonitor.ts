import * as signalR from '@microsoft/signalr';
import { writable } from 'svelte/store';
import ConfigStore from '../../store/configStore';
import {loginRedirect} from '../../services/authenticationService';
import type RobotStatusMessage from '../../models/messages/RobotStatusMessage';
import type LocationStatusMessage from '../../models/messages/LocationStatusMessage';
import TokenStore from '../../store/tokenStore';

export const robotStatusStore = writable({currentAction:{}} as RobotStatusMessage);
export const locationStatusStore = writable({} as LocationStatusMessage);
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
  
      connection.on('OnRobotStatusUpdate', (message:RobotStatusMessage) => robotStatusStore.set(message));
      connection.on('OnLocationStatusUpdate', (message:LocationStatusMessage) => locationStatusStore.set(message));
    
    }
    
    if(connection.state !== signalR.HubConnectionState.Connected)
    {
      await connection.start().catch(reason=>{
        if(reason.statusCode === 401 ||reason.statusCode === 403)
        {
          loginRedirect(config);
        }
      });
    } 
    return connection;
}
