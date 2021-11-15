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
  sendProposalCb,
  sendGuessCb,
} from "@/typings";


let guessHandlers: subscribeToGuessCb[] = [];

let guessersTurnHandlers: (() => void)[] = [];
let proposersTurnHandlers: (() => void)[] = [];

let newSliceGuesserHandlers: subscribeToNewSliceCb[] = [];
let newImageProposerHandlers: subscribeToNewImageCb[] = [];

//can we do something with generics here, this seems like too much...



const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/game")
  .build();


connection.on("Guess", (guess: Guess) => {
  guessHandlers.forEach((handler) => handler(guess));
})

connection.on("GuessersTurn", () => {
  guessersTurnHandlers.forEach((handler) => handler());
});

connection.on("ProposersTurn", () => {
  proposersTurnHandlers.forEach((handler) => handler());
});

connection.on("NewSliceGuesser", (slice : ImageSlice) => {
  newSliceGuesserHandlers.forEach((handler) => handler(slice));
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




export const sendNewGuess = (val: string): void => {
  connection.invoke("Guess", val);
}

export const sendNewProposal = (val: number): void => {
  connection.invoke("Proposal", val);
}
