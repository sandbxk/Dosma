import { User } from "../interfaces/User";
import { Buffer} from 'buffer';
import { JWToken } from "../interfaces/JWToken";
import { ConditionalExpr } from "@angular/compiler";
export class ObjectGenerator {

  public static userFromToken(token: string): User {
    let user: User = new User();
    user.token = token;

    let base64 = token.split('.');
    let payload = JSON.parse(Buffer.from(base64[1], 'base64').toString()) as JWToken;

    user.id = payload.id;
    user.username = payload.username;
    user.name = payload.name;
    user.lists = JSON.parse(payload.lists);
    user.expire_at = payload.exp;

    return user;
  }
}
