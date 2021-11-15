import axios from "axios";
import * as signalR from "@microsoft/signalr";
import {
  Image,
  ImageSlice,
  Guess,
  Proposal,
  subscribeToProposalCb,
  subscribeToNewImageCb,
  subscribeToGuessCb,
  sendProposalCb,
  sendGuessCb,
  newScore,
} from "@/typings";


let guessHandlers: subscribeToGuessCb[] = [];
let proposalHandlers: subscribeToProposalCb[] = [];

let guessersTurnHandlers: (() => void)[] = [];
let proposersTurnHandlers: (() => void)[] = [];

let newImageGuesserHandlers: (() => void)[] = [];
let newImageProposerHandlers: subscribeToNewImageCb[] = [];

let APlayerScoredHandlers: ((score: newScore) => void)[] = [];


const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/game")
  .build();


connection.on("Guess", (guess: Guess) => {
  guessHandlers.forEach((handler) => handler(guess));
});

connection.on("Proposal", (proposal: Proposal) => {
  proposalHandlers.forEach((handler) => handler(proposal));
});

connection.on("GuessersTurn", () => {
  guessersTurnHandlers.forEach((handler) => handler());
});

connection.on("ProposersTurn", () => {
  proposersTurnHandlers.forEach((handler) => handler());
});

connection.on("NewImageGuesser", () => {
  newImageGuesserHandlers.forEach((handler) => handler());
});

connection.on("NewImageProposer", (image: Image) => {
  newImageProposerHandlers.forEach((handler) => handler(image));
});

connection.on("APlayerScored", (score: newScore) => {
  APlayerScoredHandlers.forEach((handler) => handler(score));
});

connection.start();

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
  cb: ((slice: ImageSlice) => void)
) => {
  proposalHandlers = [...proposalHandlers, cb];
};

export const subscribeToNewGuess = (
  cb: ((guess: Guess) => void)
) => {
  guessHandlers = [...guessHandlers, cb];
};


export const subscribeToPlayerScores = (
  cb: ((score: newScore) => void)
) => {
  APlayerScoredHandlers = [...APlayerScoredHandlers, cb];
};

export const sendNewGuess = (val: string): void => {
  connection.invoke("Guess", val);
}

export const sendNewProposal = (val: number): void => {
  connection.invoke("Proposal", val);
}
