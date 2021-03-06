export interface User {
  id: string;
  username: string;
  isAuth: boolean;
}

interface GameSettings {
  guessersCount: number;
  imagesCount: number;
  duration: number;
}

enum GameStates {
  Created,
  Active,
  Finished,
}

export interface Game {
  id: string;
  settings: GameSettings;
  state: GameStates;
  occupiedSlotsCount: number;
}

export interface ImageFile {
  id: number;
  name: string;
  file: any;
  sliceFile: string;
  sliceColors: string[];
  category: string;
  label: string;
}

export interface Category {
  id: number;
  name: string;
}

export interface GameSlotUpdateNotification {
  gameId: string;
  occupiedSlotsCount: number;
}

export type subscribeToGameRoomsCreationCb = (game: Game) => void;
export type subscribeToGameRoomsUpdateCb = (
  data: GameSlotUpdateNotification
) => void;
export type subscribeToGameRoomsDeletionCb = (id: string) => void;
export type subscribeToGameStartCb = () => void;

export interface ImageCategory {
  id: number;
  name: string;
}

export interface ImageLabel {
  id: number;
  label: string;
  category: ImageCategory;
}

export interface ImageSlice {
  id: number;
  sequenceNumber: number;
  imageData: string;
}

export interface Image {
  id: number;
  importId: string;
  userId: string;
  label: ImageLabel;
  category: string;
  slices: ImageSlice[];
}

export type subscribeToNewImageCb = (data: Image) => void;

export type subscribeToProposalCb = (data: ImageSlice) => void;

export interface Guess {
  user: string;
  guess: string;
}

export interface Score {
  playername: string;
  score: number;
}

export interface Player {
  Name: string;
  Score: number;
  // PlayerId: string;
}

export interface CorrectGuess {
  newGuesserScore: number;
  newProposerScore: number;
  guesser: string;
  proposer: string;
  image: Image;
  guess: string;
  hasMoreRounds: boolean;
  willAutoContinue: boolean;
}

export type Proposal = ImageSlice;

export type subscribeToGuessCb = (data: Guess) => void;

export type sendProposalCb = () => number;

export type sendGuessCb = () => string;

export enum HttpStatus {
  Unathorized = 401,
}
