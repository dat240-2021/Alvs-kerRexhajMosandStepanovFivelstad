import * as signalR from "@microsoft/signalr";
import {
  guessHandlers,
  proposalHandlers,
  guessersTurnHandlers,
  proposersTurnHandlers,
  newImageGuesserHandlers,
  newImageProposerHandlers,
  scoreHandlers,
  invalidGameHandlers,
  correctGuessHandlers,
  gameOverHandlers,
  noCorrectGuessesHandlers,
} from "@/api/Game/subscriptions";
import { Image, Guess, Proposal, Score, CorrectGuess } from "@/typings";

export const gameHubConnection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/game")
  .build();

gameHubConnection.on("InvalidGame", () => {
  invalidGameHandlers.forEach((handler) => handler());
});

gameHubConnection.on("Guess", (guess: Guess) => {
  guessHandlers.forEach((handler) => handler(guess));
});

gameHubConnection.on("Proposal", (proposal: Proposal) => {
  proposalHandlers.forEach((handler) => handler(proposal));
});

gameHubConnection.on("GuessersTurn", () => {
  guessersTurnHandlers.forEach((handler) => handler());
});

gameHubConnection.on("ProposersTurn", () => {
  proposersTurnHandlers.forEach((handler) => handler());
});

gameHubConnection.on("NewImageGuesser", () => {
  newImageGuesserHandlers.forEach((handler) => handler());
});

gameHubConnection.on("NewImageProposer", (image: Image) => {
  newImageProposerHandlers.forEach((handler) => handler(image));
});

gameHubConnection.on("APlayerScored", (score: Score) => {
  scoreHandlers.forEach((handler) => handler(score));
});

gameHubConnection.on("CorrectGuess", (guess: CorrectGuess) => {
  correctGuessHandlers.forEach((handler) => handler(guess));
});

gameHubConnection.on(
  "ImageFullyVisibleWithNoCorrectGuesses",
  (guess: string) => {
    noCorrectGuessesHandlers.forEach((handler) => handler(guess));
  }
);

gameHubConnection.on("GameOver", () => {
  gameOverHandlers.forEach((handler) => handler());
});

export const sendNewGuess = (val: string): void => {
  gameHubConnection.invoke("Guess", val);
};

export const sendNewProposal = (val: number): void => {
  gameHubConnection.invoke("Propose", val);
};

export const sendConnect = (): void => {
  gameHubConnection.invoke("Connect");
};

export const sendDisconnect = (): void => {
  gameHubConnection.invoke("Disconnect");
};
