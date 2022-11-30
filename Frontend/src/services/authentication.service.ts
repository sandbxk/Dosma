import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import axios from 'axios';
import { LoginRequest, RegisterRequest, TokenResponse } from 'src/app/interfaces/User';
import { environment } from 'src/environments/environment';

const axiosInstance = axios.create({baseURL: environment.apiBaseUrl});

@Injectable({providedIn: 'root'})
export class AuthenticationService {

  headerConfig = {
    headers: {
      'Content-Type': 'application/json'
    }
  }

  constructor(private matSnackbar: MatSnackBar) { }

  async login(req : LoginRequest) {
    console.log('login');
    console.log(req);
    const httpResult = await axiosInstance.post('api/auth/login', req);
    console.log(httpResult.data);
    return httpResult.data as TokenResponse;
  }

  async register(req : RegisterRequest) {
    const httpResult = await axiosInstance.post('api/auth/register', req);
    return httpResult.data as TokenResponse;
  }
}
