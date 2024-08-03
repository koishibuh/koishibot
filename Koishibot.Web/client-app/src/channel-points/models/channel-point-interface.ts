export interface IChannelPointReward {
  twitchId: string;
  title: string;
  description: string;
  cost: number;
  backgroundColor: string;
  isEnabled: boolean;
  isUserInputRequired: boolean;
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
