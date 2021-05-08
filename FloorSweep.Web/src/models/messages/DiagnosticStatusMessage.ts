export enum DiagnosticStatus {
  Unknown = 0,
  TestingTurn = 1,
  TestingDrive = 2,
  TestingStop = 3,
  Done = 4,
}

export interface DiagnosticStatusMessage {
  status: DiagnosticStatus;
  error: String;
}
