export interface ICommand {
  id: number;
  description: string;
  enabled: boolean;
  message: string;
  globalCooldown: number;
  userCooldown: number;
  permissions: string;
  names: ICommandName[];
}

export interface ICommandName {
  id: number | null;
  name: string | null;
}

export interface IChatCommandInfo {
  commandNames: ICommandName[];
  commands: ICommand[];
}

export interface ICommandRequest {
  commandNames: ICommandName[];
  description: string;
  enabled: boolean;
  message: string;
  permissionLevel: string;
  userCooldownMinutes: number;
  globalCooldownMinutes: number;
  timerGroupIds: ICommandName[] | null;
}
