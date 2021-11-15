import { subscribeToGameRoomsCreation } from "@/api/BackendGame";

export interface User {
  id: string;
  userName: string;
  isAuth: boolean;
}

interface GameSettings {
  playersCount: number;
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

export interface GameSlotUpdateNotification {
  gameId: string;
  occupiedSlotsCount: number;
}

export type subscribeToGameRoomsCreationCb = (game: Game) => void;
export type subscribeToGameRoomsUpdateCb = (
  data: GameSlotUpdateNotification
) => void;

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
  imageData: Uint8ClampedArray;
}

export interface Image {
  id: number;
  importId: string;
  userId: string;
  label: ImageLabel;
  slices: ImageSlice[];
}

export type subscribeToNewImageCb = (
  data: Image
) => void;

export type subscribeToProposalCb = (
  data: ImageSlice
) => void;


export interface Guess {
  userId: string;
  guess: string;
}

export interface newScore {
  userId: string;
  score: number;
}


export interface Player {
  Name: string;
  Score: number;
  PlayerId: string;

}

export type Proposal = ImageSlice;

export type subscribeToGuessCb = (data: Guess) => void;

export type sendProposalCb = () => number;

export type sendGuessCb = () => string;


export enum HttpStatus {
  Unathorized = 401,
}