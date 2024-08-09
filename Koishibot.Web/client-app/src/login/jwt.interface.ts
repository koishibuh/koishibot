export interface IJwt {
  token: string;
  claims: IClaim[];
}

export interface IClaim {
  issuer: string;
  originalIssuer: string;
  type: string;
  value: string;
}
