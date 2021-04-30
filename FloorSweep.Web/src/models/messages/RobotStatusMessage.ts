export enum RobotCommandType {
  Stop = 0,
  Drive = 1,
  Turn = 2,
}
export enum RobotStatusType {
  Stopped = 0,
  Driving = 1,
  Turned = 2,
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
}
