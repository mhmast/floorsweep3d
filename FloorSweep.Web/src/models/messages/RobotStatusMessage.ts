enum RobotActionType
{
    Driving =0,
    Turning = 1,
    Stopped = 2
}

interface RobotAction{
    type:RobotActionType;
    data:number;

}

export default interface RobotStatusMessage{
    distanceToObject:number;
    currentAction:RobotAction;
};
