import type {IPollChoiceInfo} from "@/raids/models/raid-poll.interface";

export interface IPoll {
  id: string;
  title: string;
  startedAt: Date;
  endingAt: Date;
  duration: string;
  choices: IPollChoiceInfo[];
}

export interface IPollChoice {
  [key: string]: number;
}