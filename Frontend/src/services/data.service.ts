import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {GroceryList} from "../app/interfaces/GroceryList";

@Injectable({
  providedIn: 'root'
})
/*
  This service is used to share data between components.
  It is implemented in userGroceryList.component.ts and grocery-list.component.ts,
  so that the selected list is shared between the two components.
 */
export class DataService {

  private listStageObject = new BehaviorSubject({
    id: 0,
    title: '',
    items: []
  } as GroceryList);

  currentListStageObject = this.listStageObject.asObservable();

  updateListObject(list: GroceryList) {
    this.listStageObject.next(list);
  }
}
