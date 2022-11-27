import { Injectable } from '@angular/core';
import axios from "axios";
import {environment} from "../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";
import {GroceryList} from "../app/interfaces/GroceryList";

export const axiosInstance =
   axios.create({
    baseURL: environment.apiBaseUrl
  });


@Injectable({
  providedIn: 'root'
})

export class HttpGroceryListService {

  headerConfig = {
    headers: {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    }
  }

  constructor(private matSnackbar: MatSnackBar) {
    //axiosInstance.defaults.headers.common['Access-Control-Allow-Origin'] = `*`;
    //axiosInstance.defaults.headers.post['Content-Type'] = `application/json`;

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

  async createList(dto: { title: string; }) {
    const httpResult = await axiosInstance.post('GroceryList', dto);
    return httpResult.data as GroceryList;
  }

  async duplicateList(groceryListId: number) {
    //TODO implement
    const httpsResult = await axiosInstance.post(`GroceryList/${groceryListId}`);
    return httpsResult.data;
  }

  async updateList(editedList: GroceryList) {
    //TODO
    return editedList;
  }

  async deleteList(groceryList: GroceryList) {
    console.log(groceryList);
    const httpsResult = await axiosInstance.delete(`GroceryList/${groceryList.id}`, { data: groceryList });
    return httpsResult.data;
  }

}
