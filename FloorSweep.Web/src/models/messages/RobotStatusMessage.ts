export enum RobotCommandType {
  Drive = 0,
  Turn = 1,
  Stop = 2,
}
export enum RobotStatusType {
  Driving = 0,
  Turned = 1,
  Stopped = 2,
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
