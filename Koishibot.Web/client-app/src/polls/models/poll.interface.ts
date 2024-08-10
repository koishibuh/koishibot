export interface IPoll {
  id: string;
  title: string;
  startedAt: Date;
  endingAt: Date;
  duration: string;
  choices: IPollChoice;
}

export interface IPollChoice {
  [key: string]: number;
}
