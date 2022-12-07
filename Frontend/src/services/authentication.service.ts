import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import axios from 'axios';
import { LoginRequest, RegisterRequest, TokenResponse, User } from 'src/app/interfaces/User';
import { environment } from 'src/environments/environment';

const axiosInstance = axios.create({baseURL: environment.apiBaseUrl});

@Injectable({providedIn: 'root'})
export class AuthenticationService {

  constructor(private matSnackbar: MatSnackBar) { }

  async login(req : LoginRequest) : Promise<TokenResponse> {
    return axiosInstance.request({
      method: 'post',
      url: 'auth/login',
      data: req,
      headers: {
        'Content-Type': 'application/json',
      }
    }).then((response) => {
      return response.data as TokenResponse;
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return new TokenResponse();
    });
  }

  async register(req : RegisterRequest) : Promise<TokenResponse> {
    return axiosInstance.request({
      method: 'post',
      url: 'auth/register',
      data: req,
      headers: {
        'Content-Type': 'application/json',
      }
    }).then((response) => {
      return response.data as TokenResponse;
    }).catch((error) => {
      this.matSnackbar.open(error.response.data);
      return new TokenResponse();
    });
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
