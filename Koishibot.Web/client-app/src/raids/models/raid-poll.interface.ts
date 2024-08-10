export interface IRaidPoll {
  currentPollResults: IPollChoiceInfo[];
}

export interface IPollChoiceInfo {
  choice: string;
  voteCount: number;
}
