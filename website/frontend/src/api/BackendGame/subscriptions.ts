import {
  subscribeToGameRoomsCreationCb,
  subscribeToGameRoomsDeletionCb,
  subscribeToGameRoomsUpdateCb,
  subscribeToGameStartCb,
} from "@/typings";

export let createGameHandlers: subscribeToGameRoomsCreationCb[] = [];
export let updateSlotsHandlers: subscribeToGameRoomsUpdateCb[] = [];
export let deleteGameHandlers: subscribeToGameRoomsDeletionCb[] = [];
export let startGameHandlers: subscribeToGameStartCb[] = [];

/*

SUBSCRIBING METHODS

 */

export const subscribeToGameRoomsCreation = (
  cb: subscribeToGameRoomsCreationCb
) => {
  createGameHandlers = [...createGameHandlers, cb];
};

export const subscribeToGameRoomsUpdates = (
  cb: subscribeToGameRoomsUpdateCb
) => {
  updateSlotsHandlers = [...updateSlotsHandlers, cb];
};

export const subscribeToGameRoomsDeletes = (
  cb: subscribeToGameRoomsDeletionCb
) => {
  deleteGameHandlers = [...deleteGameHandlers, cb];
};

export const subscribeToGameStart = (cb: subscribeToGameStartCb) => {
  startGameHandlers = [...startGameHandlers, cb];
};

/*

UNSUBSCRIBING METHODS

 */

export const unsubscribeFromGameRoomsCreation = (
  cb: subscribeToGameRoomsCreationCb
) => {
  createGameHandlers = createGameHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeFromGameRoomsUpdates = (
  cb: subscribeToGameRoomsUpdateCb
) => {
  updateSlotsHandlers = updateSlotsHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeFromGameRoomsDeletes = (
  cb: subscribeToGameRoomsDeletionCb
) => {
  deleteGameHandlers = deleteGameHandlers.filter((handler) => handler !== cb);
};

export const unsubscribeFromGameStart = (cb: subscribeToGameStartCb) => {
  startGameHandlers = startGameHandlers.filter((handler) => handler !== cb);
};
