export interface IWordSuggestion {
  userId: number;
  username: string;
  word: string;
}

export interface IKeyboardRow {
  row: ILetterInfo[];
}

export interface IWordRow {
  letters: ILetterInfo[];
}

export interface ILetterInfo {
  letter: string;
  color: string;
}

export interface IDandleTimer {
  status: string;
  minutes: number;
  seconds: number;
}

export interface IDandleGuessedWord {
  letters: ILetterInfo[];
  keys: ILetterInfo[];
}

export interface IDandleUserScore {
  userId: number;
  username: string;
  points: number;
  bonusPoints: number;
}
