import { Injectable } from '@angular/core';
import axios from "axios";
import {environment} from "../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";
import {GroceryList} from "../app/interfaces/GroceryList";
import {MockLists} from "../app/components/user-grocery-list-overview/mockLists";
import { getToken } from 'src/app/util/storage';

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
  async getAllLists() : Promise<GroceryList[]>  {
    return axiosInstance.request({
      method: 'get',
      url: 'GroceryList',
      headers: {
        'Content-Type': 'application/json',
        'token': getToken()
      }
    }).then((response) => {
      return response.data as GroceryList[];
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return [];
    });
  }

  /**
   * Get the lists that the user has access to
   * @param userId
   */
  async getUserLists(userId: number) : Promise<GroceryList[]> {

    throw new Error('Not fully implemented in backend');
    return axiosInstance.request({
      method: 'get',
      url: 'GroceryList/' + userId,
      headers: {
        'Content-Type': 'application/json',
        'token': getToken()
      }
    }).then((response) => {
      return response.data as GroceryList[];
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return [];
    });
  }

  /**
   * Create a new list and post it to the backend
   * returns the new list with the correct id
   * @param dto
   */
  async createList(dto: { title: string; }) : Promise<GroceryList | null>{
    return axiosInstance.request({
      method: 'post',
      url: 'GroceryList',
      data: dto,
      headers: {
        'Content-Type': 'application/json',
        'token': getToken()
      }
    }).then((response) => {
      return response.data as GroceryList;
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return null;
    });
  }

  /**
   * Duplicates the list and posts it to the backend.
   * returns the duplicated list with the correct id
   * @param groceryListId
   */
  async duplicateList(groceryListId: number) {
    //TODO implement
    throw new Error('Not implemented in backend');
    const httpsResult = await axiosInstance.post(`GroceryList/${groceryListId}`);
    return httpsResult.data;
  }

  /**
   * Update the list in the backend with the new values
   *
   * @param editedList
   */
  async updateList(editedList: GroceryList) : Promise<GroceryList> {
    return axiosInstance.request({
      method: 'patch',
      url: 'GroceryList/' + editedList.id,
      data: editedList,
      headers: {
        'Content-Type': 'application/json',
        'token': getToken()
      }
    }).then((response) => {
      return response.data as GroceryList;
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return editedList;
    });
  }

  /**
   * Delete the list from the backend
   * @param groceryList
   */
  async deleteList(groceryList: GroceryList) : Promise<boolean> {
    return axiosInstance.request({
      method: 'delete',
      url: 'GroceryList/' + groceryList.id,
      data: groceryList,
      headers: {
        'Content-Type': 'application/json',
        'token': getToken()
      }
    }).then((response) => {
      return true;
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return false;
    });
  }

  /**
   * Get the list with the given id
   * @param routeId
   */
  async getListById(routeId: number) {

    throw new Error('Not fully implemented in backend');
    //TODO
    return MockLists[0];
  }
}
