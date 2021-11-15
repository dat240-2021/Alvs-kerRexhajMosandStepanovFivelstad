import { gameHubConnection } from "./BackendGame";

export default () => {
  gameHubConnection.start();
};
