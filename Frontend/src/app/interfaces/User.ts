export class User {

  id: number = 0;
  name: string = '';
  username: string = '';
  expire_at: string = '';
  token: string = '';
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
