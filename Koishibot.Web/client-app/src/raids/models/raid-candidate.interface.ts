export interface IRaidCandidate {
  streamerName: string;
  suggestedByName: string;
  streamTitle: string;
  streamGame: string;
}

export interface IRaidCandidateVm {
  multistreamUrl: string;
  raidCandidates: IRaidCandidate[];
}
