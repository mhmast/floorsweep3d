export enum RobotActionType {
  Stopped = -1,
  Stop = 0,
  Drive = 1,
  Driving = 2,
  Turn = 3,
  Turned = 4,
}

export interface RobotAction {
  type: RobotActionType;
  data: number;
}

export interface RobotStatusMessage {
  distanceToObject: number;
  currentAction: RobotAction;
}
