import { Injectable } from '@angular/core';
import {GroceryList} from "../app/interfaces/GroceryList";
import {HttpGroceryListService} from "./httpGroceryList.service";

@Injectable({
  providedIn: 'root'
})
export class SyncService {

  lastSyncedList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  invalidList: GroceryList = Object.freeze({
    id: 0,
    title: '',
    items: []
  });

  constructor(private HttpGroceryListService: HttpGroceryListService) {}

  public async syncUp(list: GroceryList) {
    if (list.id === 0 || list === this.lastSyncedList) {
      return;
    }

    else {
      this.HttpGroceryListService.updateList(list);
      this.lastSyncedList = list;
    }
  }


  public async syncDown():Promise<GroceryList> {
    if (this.lastSyncedList.id !== 0)
      return this.HttpGroceryListService.getListById(this.lastSyncedList.id);
    else
      return this.invalidList
  }



}
