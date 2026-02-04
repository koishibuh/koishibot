export interface IStreamEvent {
  eventType: string;
  timestamp: string;
  message: string;
  amount?: number | null;
}