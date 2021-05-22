export enum DiagnosticStatus {
  Unknown = 0,
  TestingTurn = 1,
  TestingDrive = 2,
  TestingStop = 3,
  DeterminingSpeed = 4,
  Done = 5,
}

export interface DiagnosticStatusMessage {
  status: DiagnosticStatus;
  error: String;
}
