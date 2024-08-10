export interface ITimer {
  minutes: number;
  seconds: number;
}

export interface IOverlayTimer {
  title: string;
  minutes: number;
  seconds: number;
}

export interface IAdTimer {
  adLength: number;
  timerEnds: Date;
}
