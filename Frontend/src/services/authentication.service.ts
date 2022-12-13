import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import axios from 'axios';
import { LoginRequest, RegisterRequest, TokenResponse } from 'src/app/interfaces/User';
import { environment } from 'src/environments/environment';
import {catchError} from "rxjs";

export const axiosInstance =
  axios.create({
    baseURL: environment.apiBaseUrl
  });

@Injectable({providedIn: 'root'})
export class AuthenticationService {

  headerConfig = {
    headers: {
      'Content-Type': 'application/json'
    }
  }

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
          matSnackbar.open(rejected.response.status + ": An error has occurred. " +
            "Please check your network connectivity and account privileges", "Dismiss", {duration: 5000});
        }
        else if (rejected.response.status > 499) { //Server error
          this.matSnackbar.open("Something went wrong", "Dismiss", {duration: 5000})
        }
        catchError(rejected);
      }
    )
  }

  async login(req : LoginRequest) {
    const httpResult = await axiosInstance.post('auth/login', req);
    return httpResult.data as TokenResponse;
  }

  async register(req : RegisterRequest) {
    const httpResult = await axiosInstance.post('auth/register', req);
    return httpResult.data as TokenResponse;
  }

  async refreshToken() {
    throw new Error('Not implemented in backend');
    const httpResult = await axiosInstance.post('auth/refresh');
    return httpResult.data as TokenResponse;
  }

  async logout() {
    localStorage.removeItem('user');
  }
}
