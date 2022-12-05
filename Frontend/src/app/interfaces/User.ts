export class User {

  id: number = 0;
  name: string = '';
  username: string = '';
  access_claims: AccessObject[] = [];
  expire_at: string = '';
  token: string = '';
}

export type AccessLevel = "READ" | "WRITE" | "DELETE";

export class AccessObject {
  access: AccessLevel[]  = [];
  resource: string = '';
  values : string[] = [];
}

export class TokenResponse {
  status: number = 0;
  error_message: string = '';
  token: string = '';
}

export class LoginRequest {
  username: string = '';
  password: string = '';
}

export class RegisterRequest {
  displayName: string = '';
  username: string = '';
  password: string = '';
}
