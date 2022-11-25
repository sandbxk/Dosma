import { Injectable } from '@angular/core';
import axios from "axios";
import {environment} from "../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";
import {GroceryList} from "../app/interfaces/GroceryList";
import {ListItem} from "../app/interfaces/ListItem";

export const axiosInstance = axios.create({
  baseURL: environment.apiBaseUrl
});


@Injectable({
  providedIn: 'root'
})

export class HttpGroceryListService {

  constructor(private matSnackbar: MatSnackBar) {
    axiosInstance.interceptors.response.use(
      response => {
        if(response.status==201) {
          this.matSnackbar.open("Great success")
        }
        return response;
      }, rejected => {
        if(rejected.response.status >=400 && rejected.response.status <500) {
          matSnackbar.open(rejected.response.data);
        }
        else if (rejected.response.status>499) {
          this.matSnackbar.open("Something went wrong")
        }
        catchError(rejected);
      }
    )
  }

  async getAllLists() {
    const httpResponse = await axiosInstance.get<any>('GroceryList');
    return httpResponse.data as GroceryList[];
  }

  async getUserLists(userId: number) {
    const httpResponse = await axiosInstance.get<any>(`GroceryList/${userId})`);
    return httpResponse.data as GroceryList[];
  }

  async createList(dto:  { title: string; listItems: ListItem[]; created: Date; modified: Date; }) {

    const httpResult = await axiosInstance.post('GroceryList', dto);
    return httpResult.data;
  }

  async updateList(editedList: any) {
    //TODO
    return editedList;
  }

  async deleteList(groceryListId: number) {
    const httpsResult = await axiosInstance.delete(`GroceryList/${groceryListId}`);
    return httpsResult.data;
  }

}
