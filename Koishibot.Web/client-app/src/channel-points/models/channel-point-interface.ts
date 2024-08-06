export interface IChannelPointReward {
  twitchId: string;
  title: string;
  cost: number;
  isEnabled: boolean;
  isUserInputRequired: boolean;
  description: string;
  backgroundColor: string;
  isMaxPerStreamEnabled: boolean;
  maxPerStream: number;
  isMaxPerUserPerStreamEnabled: boolean;
  maxPerUserPerStream: number;
  isGlobalCooldownEnabled: boolean;
  globalCooldownSeconds: number;
  isPaused: boolean;
  shouldRedemptionsSkipRequestQueue: boolean;
  imageUrl: string;
}
