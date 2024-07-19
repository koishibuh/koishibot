import {
  HubConnectionState,
  HubConnectionBuilder,
  HubConnection,
  LogLevel
} from '@microsoft/signalr';
import { ref, computed } from 'vue';
const signalRConnections = ref(new Map<string, HubConnection>());

export const useSignalR = () => {
  const signalRHubStatuses = computed(() => {
    const statuses = new Map<string, HubConnectionState>();
    signalRConnections.value.forEach((connection, hubName) => {
      statuses.set(hubName, connection.state);
    });
    return statuses;
  });

  const createSignalRConnection = async (hubName: string): Promise<HubConnection> => {
    const existingConnection = signalRConnections.value.get(hubName);

    if (
      signalRConnections.value.has(hubName) &&
      signalRConnections.value.get(hubName)?.state === HubConnectionState.Connected
    ) {
      return existingConnection as HubConnection;
    }

    const signalRConnection = new HubConnectionBuilder()
      .withUrl(`/${hubName}`)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Debug)
      .build();

    signalRConnection.keepAliveIntervalInMilliseconds = 1000;

    try {
      await startSignalRConnection(signalRConnection);
    } catch (error) {
      console.error('Error starting signalR connection', error);
    }

    signalRConnections.value.set(hubName, signalRConnection);
    return signalRConnection;
  };

  const startSignalRConnection = async (connection: HubConnection): Promise<void> => {
    if (!connection) {
      throw new Error('No signalR connection found');
    }
    console.log('StartSignalRConnection', 'Starting SignalR connection');
    connection.keepAliveIntervalInMilliseconds = 1000;
    await connection.start();
  };

  const getConnectionByHub = (hubName: string): HubConnection | null => {
    return (signalRConnections.value.get(hubName) as HubConnection) ?? null;
  };

  const reconnectHub = () => {
    signalRConnections.value.forEach(async (connection) => {
      if (connection.state === HubConnectionState.Disconnected) {
        await startSignalRConnection(connection as HubConnection);
        console.log('ReconnectSignalRConnection', 'SignalR reconnected');
      }
    });
  };

  return {
    createSignalRConnection,
    getConnectionByHub,
    reconnectHub,
    signalRHubStatuses
  };
};
