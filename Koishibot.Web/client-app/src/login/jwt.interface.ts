export interface IJwt {
  token: string;
  claims: IClaim[];
}

export interface IClaim {
  type: string;
  value: string;
}
