import { Injectable } from '@angular/core';
import axios from "axios";
import {environment} from "../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";
import {GroceryList} from "../app/interfaces/GroceryList";
import {MockLists} from "../app/components/user-grocery-list-overview/mockLists";

export const axiosInstance =
   axios.create({
    baseURL: environment.apiBaseUrl
  });


@Injectable({
  providedIn: 'root'
})

/**
 * Service for interacting with the backend
 */
export class HttpGroceryListService {


  constructor(private matSnackbar: MatSnackBar) {
    axiosInstance.interceptors.response.use(
      response => {
        if(response.status==201) { //Created
          this.matSnackbar.open("Great success")
        }
        return response;
      },
      rejected => {
        if(rejected.response.status >= 400 && rejected.response.status < 500) { //Client error
          matSnackbar.open(rejected.response.data);
        }
        else if (rejected.response.status > 499) { //Server error
          this.matSnackbar.open("Something went wrong")
        }
        catchError(rejected);
      }
    )
  }

  /**
   * Get all lists in the system -- Strictly for testing purposes
   */
  async getAllLists() {
    const httpResponse = await axiosInstance.get<any>('GroceryList');
    return httpResponse.data as GroceryList[];
  }

  /**
   * Get the lists that the user has access to
   * @param userId
   */
  async getUserLists(userId: number) {
    const httpResponse = await axiosInstance.get<any>(`GroceryList/${userId})`);
    return httpResponse.data as GroceryList[];
  }

  /**
   * Create a new list and post it to the backend
   * returns the new list with the correct id
   * @param dto
   */
  async createList(dto: { title: string; }) {
    const httpResult = await axiosInstance.post('GroceryList', dto);
    return httpResult.data as GroceryList;
  }

  /**
   * Duplicates the list and posts it to the backend.
   * returns the duplicated list with the correct id
   * @param groceryListId
   */
  async duplicateList(groceryListId: number) {
    //TODO implement
    const httpsResult = await axiosInstance.post(`GroceryList/${groceryListId}`);
    return httpsResult.data;
  }

  /**
   * Update the list in the backend with the new values
   *
   * @param editedList
   */
  async updateList(editedList: GroceryList) {
    //TODO
    return editedList;
  }

  /**
   * Delete the list from the backend
   * @param groceryList
   */
  async deleteList(groceryList: GroceryList) {
    const httpsResult = await axiosInstance.delete(`GroceryList/${groceryList.id}`, { data: groceryList });
    return httpsResult.data;
  }

  /**
   * Get the list with the given id
   * @param routeId
   */
  async getListById(routeId: number) {
    //TODO
    return MockLists[0];
  }
}
