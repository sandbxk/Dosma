import { Injectable } from '@angular/core';
import { AccessLevel, AccessObject, AccessResources, User } from 'src/app/interfaces/User';

type WrappedBool = {success: boolean, access: undefined};
type AccessList = {success : boolean, access: AccessObject[]};
type AccessSingle = {success : boolean, access: AccessObject};
type LevelUndefined = AccessLevel | undefined;

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }

  /**
   * @returns True if the user is logged in, false otherwise
   * @see User
   */
  isLoggedIn() : boolean {
    let user : User | null = JSON.parse(localStorage.getItem('user') as string);

    if (user) {
      // TODO: check if time is expired
      return true;
    }
    return false;
  }

  /**
   * @param listID The ID of the list
   * @returns True if the user has read access the list, false otherwise
   * @see hasAccess
   */
  canReadList(listID : number) : boolean {
    return this.hasAccess("LIST", listID.toString(), "READ");
  }

  /**
   * @param listID The ID of the list
   * @returns True if the user can add, delete and update items in the list, false otherwise
   * @see hasAccess
   */
  canWriteList(listID : number) : boolean {
    return this.hasAccess("LIST", listID.toString(), "WRITE");
  }

  /**
   * @param listID The ID of the list
   * @returns True if the user can delete the list, false otherwise
   * @see hasAccess
   */
  canDeleteList(listID : number) : boolean {
    return this.hasAccess("LIST", listID.toString(), "DELETE");
  }

  get userDisplayName() : string {
    const user = JSON.parse(localStorage.getItem('user') as string) as User;

    return user.name;
  }

  /*
  ****************************************************************************
  * Private parts
  *
  */

  /**
   *
   * @param resource The path to the resource [LIST, ...]
   * @param value Value of the resource
   * @param level The supported action that can be executed by the current user for this resource [READ, WRITE, DELETE]
   * @returns True if the user has access to the resource, false otherwise
   * @see hasClaim @see AccessObject @see AccessResources @see AccessLevel
   */
  private hasAccess(resource : AccessResources, value : string, level: LevelUndefined = undefined) : boolean {
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

  /**
   * @param resource The path to the resource [LIST, ...]
   * @param level The supported action that can be executed by the current user for this resource [READ, WRITE, DELETE]
   * @returns True if the user has access to the resource, false otherwise.
   *          And the access object if the user has access to the resource.
   * @see AccessResources @see AccessLevel @see AccessObject @see User
   */
  private hasClaim(resource : AccessResources, level: LevelUndefined = undefined) : WrappedBool | AccessSingle | AccessList {
    let user : User | null = JSON.parse(localStorage.getItem('user') as string);

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
