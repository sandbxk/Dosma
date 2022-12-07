import { User } from "../interfaces/User";

export function getToken() : string {
  return (JSON.parse(localStorage.getItem('user') as string) as User).token;
}

export function getUser() : User {
  return JSON.parse(localStorage.getItem('user') as string) as User;
}
