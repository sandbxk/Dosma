export interface JWTUser {

  header: string;

  id: number;
  name: string;
  username: string;

  //TBA created: Date;
  //TBA modified: Date;

  signature: string;
}

export interface TokenResponse {
  status: number;
  error_message: string;
  token: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  username: string;
  password: string;
  passwordConfirm: string;
}
