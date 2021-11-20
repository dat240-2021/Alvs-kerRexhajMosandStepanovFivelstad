import {
  subscribeToGuessCb,
  subscribeToProposalCb,
  subscribeToNewImageCb,
  Score,
  Image,
  Guess,
  Proposal,
} from "@/typings";

export let invalidGameHandlers: (() => void)[] = [];
export let guessHandlers: subscribeToGuessCb[] = [];
export let proposalHandlers: subscribeToProposalCb[] = [];

export let guessersTurnHandlers: (() => void)[] = [];
export let proposersTurnHandlers: (() => void)[] = [];

export let newImageGuesserHandlers: (() => void)[] = [];
export let newImageProposerHandlers: subscribeToNewImageCb[] = [];

export let scoreHandlers: ((score: Score) => void)[] = [];

/*

SUBSCRIBING METHODS

 */

export const subscribeToInvalidGame = (
  cb: (() => void)
) => {
  invalidGameHandlers = [...invalidGameHandlers, cb];
};

export const subscribeToGuessersTurn = (
  cb: (() => void)
) => {
  guessersTurnHandlers = [...guessersTurnHandlers, cb];
};

export const subscribeToProposersTurn = (
  cb: (() => void)
) => {
  proposersTurnHandlers = [...proposersTurnHandlers, cb];
};

export const subscribeToNewImageGuesser = (
  cb: (() => void)
) => {
  newImageGuesserHandlers = [...newImageGuesserHandlers, cb];
};

export const subscribeToNewImageProposer = (
  cb: ((image: Image) => void)
) => {
  newImageProposerHandlers = [...newImageProposerHandlers, cb];
};

export const subscribeToNewProposal = (
  cb: ((slice: Proposal) => void)
) => {
  proposalHandlers = [...proposalHandlers, cb];
};

export const subscribeToNewGuess = (
  cb: ((guess: Guess) => void)
) => {
  guessHandlers = [...guessHandlers, cb];
};


export const subscribeToPlayerScores = (
  cb: ((score: Score) => void)
) => {
  scoreHandlers = [...scoreHandlers, cb];
};

/*

UNSUBSCRIBING METHODS

 */

export const unsubscribeToInvalidGame = (
  cb: (() => void)
) => {
  invalidGameHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToGuessersTurn = (
  cb: (() => void)
) => {
  guessersTurnHandlers = guessersTurnHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToProposersTurn = (
  cb: (() => void)
) => {
  proposersTurnHandlers = proposersTurnHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToNewImageGuesser = (
  cb: (() => void)
) => {
  newImageGuesserHandlers = newImageGuesserHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToNewImageProposer = (
  cb: ((image: Image) => void)
) => {
  newImageProposerHandlers = newImageProposerHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToNewProposal = (
  cb: ((slice: Proposal) => void)
) => {
  proposalHandlers = proposalHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToNewGuess = (
  cb: ((guess: Guess) => void)
) => {
  guessHandlers = guessHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeToPlayerScores = (
  cb: ((score: Score) => void)
) => {
  scoreHandlers = scoreHandlers.filter((handler) => handler !== cb);
};