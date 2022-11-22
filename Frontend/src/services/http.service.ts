import { Injectable } from '@angular/core';
import axios from "axios";
import {environment} from "../environments/environment";
import {MatSnackBar} from "@angular/material/snack-bar";
import {catchError} from "rxjs";

export const axiosInstance = axios.create({
  baseURL: environment.apiBaseUrl
});


@Injectable({
  providedIn: 'root'
})

export class HttpService {

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

  async getUserLists(){
    //TODO
  }


}
