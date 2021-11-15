export interface User {
  id: string;
  userName: string;
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

export enum HttpStatus {
  Unathorized = 401,
}
