import { User } from "../interfaces/User";
import { Buffer} from 'buffer';
import { JWToken } from "../interfaces/JWToken";
export class ObjectGenerator {

  public static userFromToken(token: string): User {
    let user: User = new User();
    user.token = token;

    let base64 = token.split('.');
    let payload = JSON.parse(Buffer.from(base64[1], 'base64').toString()) as JWToken;

    user.id = payload.userId;
    user.username = payload.username;
    user.name = payload.name;
    user.expire_at = payload.exp;

    // parse JWT token
    console.log(payload);
    return user;
  }
}
