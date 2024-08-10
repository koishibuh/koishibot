import { ref } from 'vue';
import { defineStore } from 'pinia';
import { useSignalR } from '@/api/signalr.composable';
import type {
  IWordSuggestion,
  IWordRow,
  IDandleTimer,
  IKeyboardRow,
  IDandleGuessedWord,
  ILetterInfo,
  IDandleUserScore
} from '@/dandle/dandle.interface';
import { type IPollChoiceInfo } from '@/raids/models/raid-poll.interface';
import keyboardDefault from '@/dandle/data/keyboard-default.json';
import guessBoard from '@/dandle/data/guessboard-default.json';
import wordGuessData from '@/dandle/sample/wordVoteGuessData.json';
import wordSuggestionsData from '@/dandle/sample/wordSuggestionsData.json';

export const useDandleStore = defineStore('dandleStore', () => {
  const { getConnectionByHub } = useSignalR();
  const signalRConnection = getConnectionByHub('notifications');

  const keyboard = ref<IKeyboardRow[]>(keyboardDefault);

  const enableSuggestions = ref<boolean>(true);
  const suggestedWords = ref<IWordSuggestion[]>([]); /*  (wordSuggestionsData); */

  const enablePoll = ref<boolean>(false);
  const dandleVotes = ref<IPollChoiceInfo[]>([]); /* (Data); */
  const voteCountTotal = ref<number>(0);

  const guessedWords = ref<IWordRow[]>(new Array(6).fill(guessBoard));
  const guessedWordCount = ref<number>(0);

  const dandleTimer = ref<IDandleTimer>({ status: '!GUESS A WORD', minutes: 0, seconds: 0 });
  const newTimer = ref<Boolean>(true);
  const displayTimer = ref<boolean>(false);

  const scoreBoard = ref<IDandleUserScore[]>([]);

  const message = ref<string>('');

  ///

  signalRConnection?.on('ReceiveDandleWordSuggestion', (wordSuggestion: IWordSuggestion) => {
    suggestedWords.value.push(wordSuggestion);
    if (suggestedWords.value.length > 3) {
      suggestedWords.value.shift();
    }
  });

  signalRConnection?.on('ReceiveDandleScore', (userScore: IDandleUserScore[]) => {
    userScore.forEach((user) => {
      const oldUser = scoreBoard.value.find((x) => x.userId === user.userId);
      if (oldUser) {
        console.log('olduser', user.points);
        oldUser.points = user.points;
      } else {
        console.log('newuser', user.points);
        scoreBoard.value.push(user);
      }
    });
  });

  signalRConnection?.on('ReceiveDandleGuessChoices', (guessChoices: IPollChoiceInfo[]) => {
    suggestedWords.value = [];
    enableSuggestions.value = false;
    dandleVotes.value = guessChoices;
    enablePoll.value = true;
    voteCountTotal.value = 0;
    console.log('ReceiveDandleGuessChoices', guessChoices);
  });

  signalRConnection?.on('ReceiveDandleVote', (dandleVote: IPollChoiceInfo) => {
    console.log('received vote', dandleVote);
    const word = dandleVotes.value?.find((x) => x.choice === dandleVote.choice);
    if (word) {
      word.voteCount = word.voteCount + 1;
      voteCountTotal.value = voteCountTotal.value + 1;
    }
  });

  signalRConnection?.on('ReceiveClearDandleSuggestions', () => {
    suggestedWords.value = [];
    console.log('Cleared Suggestion List');
  });

  signalRConnection?.on('ReceiveDandleWordGuess', (Data: IDandleGuessedWord) => {
    console.log('received dandle word guess', Data);
    enableSuggestions.value = true;
    enablePoll.value = false;
    voteCountTotal.value = 0;
    dandleVotes.value = [];

    guessedWords.value.splice(guessedWordCount.value, 1, { letters: Data.letters });

    if (guessedWordCount.value < 6) {
      guessedWordCount.value = guessedWordCount.value + 1;
      console.log(guessedWordCount.value);
    } else {
      guessedWordCount.value = 0;
    }

    console.log('Data', Data.keys);
    console.log('keyboard', keyboard.value);

    const guessedWordKeys = { keys: Data.keys };

    keyboard.value = keyboard.value.map((row: IKeyboardRow) => {
      return {
        row: row.row.map((letterInfo: ILetterInfo) => {
          const matchingKey = guessedWordKeys.keys.find(
            (key: ILetterInfo) => key.letter === letterInfo.letter
          );
          return matchingKey || letterInfo;
        })
      };
    });
  });

  signalRConnection?.on('ReceiveDandleTimer', (timer: IDandleTimer) => {
    dandleTimer.value = timer;
    if (timer.minutes === 0 && timer.seconds === 0) {
      displayTimer.value = false;
    }
  });

  signalRConnection?.on('ReceiveDandleMessage', (dandleMessage: string) => {
    message.value = dandleMessage;
  });

  signalRConnection?.on('ReceiveClearDandleBoard', () => {
    enablePoll.value = false;
    dandleTimer.value = { status: '!GUESS A WORD', minutes: 0, seconds: 0 };
    dandleVotes.value = [];
    voteCountTotal.value = 0;
    guessedWords.value = [];
    guessedWords.value = new Array(6).fill(guessBoard);
    guessedWordCount.value = 0;
    scoreBoard.value = [];
    keyboard.value = keyboardDefault;
    enableSuggestions.value = true;
  });

  return {
    suggestedWords,
    guessedWords,
    guessedWordCount,
    dandleTimer,
    newTimer,
    dandleVotes,
    voteCountTotal,
    keyboard,
    enableSuggestions,
    enablePoll,
    scoreBoard,
    message,
    displayTimer
  };
});
