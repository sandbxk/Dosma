import { Injectable } from '@angular/core';
import axios from 'axios';
import { environment } from '../environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError } from 'rxjs';
import { GroceryList } from '../app/interfaces/GroceryList';
import { Status } from '../app/interfaces/StatusEnum';
import { User } from '../app/interfaces/User';
import { Item, ItemDTO } from '../app/interfaces/Item';
import { CategoryEnum } from '../app/interfaces/CategoryEnum';

export const axiosInstance = axios.create({
  baseURL: environment.apiBaseUrl,
});

@Injectable({
  providedIn: 'root',
})

/**
 * Service for interacting with the backend
 */
export class HttpGroceryListService {
  constructor(private matSnackbar: MatSnackBar) {
    axiosInstance.interceptors.response.use(
      (response) => {
        if (response.status == 201) {
          //Created
          this.matSnackbar.open('Great success', 'Dismiss', { duration: 5000 });
        }
        return response;
      },
      (rejected) => {
        if (rejected.response.status >= 400 && rejected.response.status < 500) {
          //Client error
          matSnackbar.open(
            rejected.response.status +
              ': An error has occurred. ' +
              'Please check your network connectivity and account privileges',
            'Dismiss',
            { duration: 5000 }
          );
        } else if (rejected.response.status > 499) {
          //Server error
          this.matSnackbar.open('Something went wrong', 'Dismiss', {
            duration: 5000,
          });
        }
        catchError(rejected);
      }
    );
  }

  /**
   * Get the lists that the user has access to
   * @param userId
   */
  async getAllLists() {
    try {
      const header = this.configHeader();
      const httpResponse = await axiosInstance.get<any>('GroceryList', header);
      return httpResponse.data as GroceryList[];
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      return [];
    }
  }

  /**
   * Create a new list and post it to the backend
   * returns the new list with the correct id
   * @param dto
   */
  async createList(dto: { title: string }): Promise<GroceryList> {
    try {
      const header = this.configHeader();
      return await axiosInstance
        .post('GroceryList', dto, header)
        .then((response) => {
          return response.data as GroceryList;
        });
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      let error: GroceryList = {
        id: 0,
        title: 'Error',
        items: [],
      };
      return error;
    }
  }

  /**
   * Update the list in the backend with the new values
   *
   * @param editedList
   */
  async updateList(editedList: {
    id: number;
    title: string;
  }): Promise<GroceryList> {
    try {
      const header = this.configHeader();
      const httpResult = await axiosInstance.patch(
        'GroceryList/',
        editedList,
        header
      );
      return httpResult.data as GroceryList;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      throw new Error('You must be logged in to update lists');
    }
  }

  async createItem(item: {
    quantity: number;
    title: string;
    category: string;
    status: Status;
    groceryListId: number;
  }): Promise<ItemDTO> {
    try {
      const header = this.configHeader();

      return await axiosInstance.post('item', item, header).then((response) => {
        return response.data as ItemDTO;
      });
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      throw new Error('You must be logged in to create items');
    }
  }

  async updateItem(item: ItemDTO): Promise<ItemDTO> {
    try {
      const header = this.configHeader();
      const httpResult = await axiosInstance.patch('item', item, header);
      return httpResult.data as ItemDTO;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      throw new Error('You must be logged in to update lists');
    }
  }

  async deleteItem(itemId: number) {
    try {
      const header = this.configHeader();
      await axiosInstance.delete(`item/${itemId}`, header); //Errors handled in interceptor
      return true;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      return false;
    }
  }

  /**
   * Delete the list from the backend
   * @param groceryList
   */
  async deleteList(groceryListId: number) {
    try {
      const header = this.configHeader();
      await axiosInstance.delete(`GroceryList/${groceryListId}`, header); //Errors handled in interceptor
      return true;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      return false;
    }
  }

  /**
   * Get the list with the given id
   * @param routeId
   */
  async getListById(routeId: number): Promise<GroceryList> {
    try {
      const header = this.configHeader();
      const httpsResult = await axiosInstance.get(
        `GroceryList/${routeId}`,
        header
      );
      return httpsResult.data as GroceryList;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      let error: GroceryList = {
        id: 0,
        title: 'Error',
        items: [],
      };
      return error;
    }
  }

  getCategories() {
    return Object.values(CategoryEnum).filter(
      (value) => typeof value === 'string'
    ) as string[];
  }

  async duplicateItem(duplicateDTO: {
    quantity: number;
    title: string;
    category: string;
    status: Status;
    groceryListId: number;
  }) {
    try {
      const header = this.configHeader();
      const httpsResult = await axiosInstance.post(
        `Item`,
        duplicateDTO,
        header
      );
      return httpsResult.data as Item;
    } catch (e) {
      if (e instanceof Error) {
        this.matSnackbar.open(e.message, 'Dismiss', {
          duration: 5000,
        });
      } else {
        this.matSnackbar.open('Something went wrong', 'Dismiss', {
          duration: 5000,
        });
      }
      let error: Item = {
        id: 0,
        title: 'Error',
        status: Status.Unchecked,
        quantity: 0,
        category: 'None',
        groceryListId: 0,
        index: 0,
      };
      return error;
    }
  }

  private configHeader() {
    const user: User = JSON.parse(localStorage.getItem('user') as string);
    if (user) {
      const token = user.token;
      return {
        headers: {
          contentType: 'application/json',
          token: `${token}`,
        },
      };
    } else throw new Error('User not logged in');
  }
}
