import { User } from "../interfaces/User";

export class ObjectGenerator {

  public static userFromToken(token: string): User {
    let user: User = new User();
    user.token = token;

    // parse JWT token

    return user;
  }
}
