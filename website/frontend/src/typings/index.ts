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

interface WaitingEntry {
  id: string;
  gameId: string;
}

export interface Game {
  id: string;
  settings: GameSettings;
  state: GameStates;
  waitingPool: WaitingEntry[];
}

export enum HttpStatus {
  Unathorized = 401,
}
