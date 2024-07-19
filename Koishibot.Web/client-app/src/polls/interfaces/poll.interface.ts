import { type IPollChoice } from './poll-choice.interface';

export interface IPoll {
  id: string;
  title: string;
  startedAt: Date;
  endingAt: Date;
  duration: string;
  choices: IPollChoice;
}
