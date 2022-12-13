import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { LoginRequest, User } from 'src/app/interfaces/User';
import { ObjectGenerator } from 'src/app/util/ObjectGenerator';
import { AuthenticationService } from 'src/services/authentication.service';
import { RegisterComponent } from '../register/register.component';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginRequest : LoginRequest = new LoginRequest();
  rememberMe: boolean = false;
  error: string | any = '';

  login() {
    this.authService.login(this.loginRequest).then((result) => {
        // handle success
        localStorage.setItem('user', JSON.stringify(ObjectGenerator.userFromToken(result.token)));
        this.router.navigate(['/dashboard']);
        this.loginForm.close();
      }).catch((error) => {
        // mat snackbar handled in auth service
        this.error = error;

      }).finally(() => {
        // save login data to local storage if remember me is checked regardless of login success
        if (this.rememberMe) {
          localStorage.setItem('rememberedLogin', JSON.stringify(this.loginRequest));
        } else {
          localStorage.removeItem('rememberedLogin');
        }
      }
    );
  }

  cancel() {
    this.loginForm.close();
  }

  openRegisterForm() : void
  {
    this.loginForm.close();
    this.dialog.open(RegisterComponent);
  }

  openForgotPasswordForm() {
    throw new Error('Method not implemented.');
  }

  toggleRememberMe() {
    this.rememberMe = !this.rememberMe;
  }

  constructor(
    private authService: AuthenticationService,
    private router : Router,
    public loginForm: MatDialogRef<LoginComponent>,
    private dialog: MatDialog
  ) {}

  private loadRememberMe() : void{
    let remembered : LoginRequest | null = JSON.parse(localStorage.getItem("rememberedLogin") as string);

    if (remembered) {
      this.loginRequest = remembered;
      this.rememberMe = true;
    } else {
      this.rememberMe = false;
    }
  }

  private loadSavedToken() : boolean {
    let user : User | null = JSON.parse(localStorage.getItem('user') as string);

    if (user) {
      console.log('token found in local storage');
      // TODO: check if token is valid
        // true -> TODO: auto login & navigate to dashboard & return true
        // false -> TODO: remove token from local storage & return false
      return false;
    }

    return false;
  }

  clearError() {
    this.error = '';
  }

  ngOnInit(): void {
    if (this.loadSavedToken()) {
      this.router.navigate(['/dashboard']);
    } else {
      this.loadRememberMe();
    }
  }
}
