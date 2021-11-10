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

export enum HttpStatus {
  Unathorized = 401,
}
