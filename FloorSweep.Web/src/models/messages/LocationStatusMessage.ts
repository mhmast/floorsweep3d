export enum LocationDeterminationStatus {
  Unknown = -1,
  SpeedTesting = 0,
  Orienting = 1,
  LocationInSync = 2,
}

export interface LocationStatusMessage {
  avgSpeedPixelsPerSecond: number;
  lastUpdateReceived: Date;
  locationDeterminationStatus: LocationDeterminationStatus;
}
