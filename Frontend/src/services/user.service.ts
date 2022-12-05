import { Injectable } from '@angular/core';
import { AccessLevel, AccessObject, User } from 'src/app/interfaces/User';

type WrappedBool = {success: boolean, access: undefined};
type AccessList = {success : boolean, access: AccessObject[]};
type AccessSingle = {success : boolean, access: AccessObject};
type LevelUndefined = AccessLevel | undefined;

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  isLoggedIn() : boolean {
    let user : User | null = JSON.parse(localStorage.getItem('user') || '{}');

    if (user) {
      // TODO: check if time is expired
      return true;
    }

    return false;
  }



  hasAccess(resource : string, value : string, level: LevelUndefined = undefined) : boolean {
    let claim = this.hasClaim(resource, level);

    if (claim.success) {

      if (level)
      {
        return (claim.access as AccessObject).values.includes(value);
      }

      return (claim.access as AccessObject[]).findIndex((obj) => obj.values.includes(value)) > -1;
    }

    return false
  }

  hasClaim(resource : string, level: LevelUndefined = undefined) : WrappedBool | AccessSingle | AccessList {
    let user : User | null = JSON.parse(localStorage.getItem('user') || '{}');

    if (user) {

      let matches = user.access_claims.filter((value) => value.resource === resource);

      if (matches.length > 0) {
        if (level) {
          return {
            success: true,
            access: matches.find(obj => obj.access.includes(level))
          }
        }
        else {
          return {
            success: true,
            access: matches
          }
        }
      }
    }

    return {success: false, access: undefined};
  }

}
