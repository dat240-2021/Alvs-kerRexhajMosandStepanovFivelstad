export interface User {
  id: string;
  userName: string;
  isAuth: boolean;
}

export enum HttpStatus {
  Unathorized = 401,
}
