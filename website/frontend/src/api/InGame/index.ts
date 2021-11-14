import axios from "axios";
import * as signalR from "@microsoft/signalr";
import {
  Image,
  Guess,
  Proposal,
  subscribeToNewImageCb,
  subscribeToGuessCb,
  subscribeToProposalCb
} from "@/typings";

let guessHandlers: subscribeToGuessCb[] = [];
let proposalHandlers: subscribeToProposalCb[] = [];

let guessersTurnHandlers: (() => void)[] = [];
let proposersTurnHandlers: (() => void)[] = [];
let newImageGuesserHandlers: (() => void)[] = [];
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

connection.on("NewImageGuesser", () => {
  newImageGuesserHandlers.forEach((handler) => handler());
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
