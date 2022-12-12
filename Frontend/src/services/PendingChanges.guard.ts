import { CanDeactivate } from '@angular/router';
import { Injectable } from '@angular/core';
import {Observable} from "rxjs";

/**
 * Interface that a class can implement to be a guard deciding if a route can be deactivated.
 * If all guards return true, navigation continues.
 * If any guard returns false, navigation is cancelled.
 * If any guard returns a UrlTree, current navigation is cancelled
 * and a new navigation begins to the UrlTree returned from the guard.
 */
export interface IComponentCanDeactivate {
  canDeactivate: () => boolean | Observable<boolean>;
}

/**
 * A guard that can be used to check if a component can be deactivated.
 *  If all guards return true, navigation continues.
 *  If any guard returns false, navigation is cancelled and a confirmation dialog is shown.
 */
@Injectable()
export class PendingChangesGuard implements CanDeactivate<IComponentCanDeactivate> {
  canDeactivate(component: IComponentCanDeactivate): boolean | Observable<boolean> {
    // if there are no pending changes, just allow deactivation; else confirm first
    return component.canDeactivate() ?
      true :
      // NOTE: this warning message will only be shown when navigating elsewhere within your angular app;
      // when navigating away from your angular app, the browser will show a generic warning message
      // see http://stackoverflow.com/a/42207299/7307355
      confirm('WARNING: Your changes are still being synchronized. Press Cancel to go back and save these changes, or OK to lose these changes.');
  }
}
