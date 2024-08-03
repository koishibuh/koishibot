export interface IChannelRewardRequest {
  title: string;
  cost: number;
  isEnabled: boolean;
  isUserInputRequired: boolean;
  prompt: string;
  backgroundColor: string;
  isMaxPerStreamEnabled: boolean;
  maxPerStream: number;
  isMaxPerUserPerStreamEnabled: boolean;
  maxPerUserPerStream: number;
  isGlobalCooldownEnabled: boolean;
  globalCooldownSeconds: number;
  shouldRedemptionsSkipRequestQueue: boolean;
}
