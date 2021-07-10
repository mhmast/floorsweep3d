export enum RobotCommandType {
  Drive = "Drive",
  Turn = "Turn",
  Stop = "Stop",
}
export enum RobotStatusType {
  Driving = "Driving",
  Turned = "Turned",
  Stopped = "Stopped",
}
export interface RobotCommandMessage {
  type: RobotCommandType;
  data: number;
}

export interface RobotAction {
  type: RobotStatusType;
  data: number;
}

export interface RobotStatusMessage {
  distanceToObject: number;
  currentAction: RobotAction;
  statusDate: Date;
}
