import axios from "axios";
import * as signalR from "@microsoft/signalr";
import {
  Image,
  ImageSlice,
  Guess,
  Proposal,
  subscribeToNewSliceCb,
  subscribeToNewImageCb,
  subscribeToGuessCb,
  subscribeToProposalCb
} from "@/typings";

let guessHandlers: subscribeToGuessCb[] = [];
let proposalHandlers: subscribeToProposalCb[] = [];

let guessersTurnHandlers: (() => void)[] = [];
let proposersTurnHandlers: (() => void)[] = [];

let newSliceGuesserHandlers: subscribeToNewSliceCb[] = [];
let newImageProposerHandlers: subscribeToNewImageCb[] = [];



const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/game")
  .build();

connection.on("Guess", (guess: Guess) => {
  guessHandlers.forEach((handler) => handler(guess));
})

connection.on("Proposal", (proposal: Proposal) => {
  proposalHandlers.forEach((handler) => handler(proposal));
});

connection.on("GuessersTurn", () => {
  guessersTurnHandlers.forEach((handler) => handler());
});

connection.on("ProposersTurn", () => {
  proposersTurnHandlers.forEach((handler) => handler());
});

connection.on("NewSliceGuesser", () => {
  newSliceGuesserHandlers.forEach((handler) => handler());
});

connection.on("NewImageProposer", (image: Image) => {
  newImageProposerHandlers.forEach((handler) => handler(image));
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

export const SubscribeToNewImageProposer = (
  cb: ((image: Image) => void)
) => {
  newImageProposerHandlers = [...newImageProposerHandlers, cb];
};

export const SubscribeToNewSliceGuesser = (
  cb: ((slice: ImageSlice) => void)
) => {
  newSliceGuesserHandlers = [...newSliceGuesserHandlers, cb];
};



export const SubscribeToNewGuess = (
  cb: ((guess: Guess) => void)
) => {
  guessHandlers = [...guessHandlers, cb];
};

export const SubscribeToNewProposal = (
  cb: ((proposal: Proposal) => void)
) => {
  proposalHandlers = [...proposalHandlers, cb];
};