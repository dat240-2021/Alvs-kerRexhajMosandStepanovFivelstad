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

export enum HttpStatus {
  Unathorized = 401,
}
