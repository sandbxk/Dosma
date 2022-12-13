import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { RegisterRequest, User } from 'src/app/interfaces/User';
import { ObjectGenerator } from 'src/app/util/ObjectGenerator';
import { AuthenticationService } from 'src/services/authentication.service';
import { LoginComponent } from '../login/login.component';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import { FormCustomValidators } from 'src/app/util/formCustomValidators';

@Component({
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {

  request : RegisterRequest = new RegisterRequest();

  confirm_password : string = '';
  registerFormControlGroup: FormGroup = new FormGroup({})


  constructor(
    private authService : AuthenticationService,
    private router: Router,
    public registerForm: MatDialogRef<RegisterComponent>,
    private dialog: MatDialog,
    ) { }

  ngOnInit(): void {
    this.registerFormControlGroup = new FormGroup({
      username: new FormControl(this.request.username, [
        Validators.required,
        Validators.minLength(3), // minimum length of 3
        Validators.maxLength(20), // maxLength of 20 characters
        Validators.pattern('^[a-zA-Z0-9]+$') //alphanumeric only
      ]),

      displayName: new FormControl('', []),

      password: new FormControl('',[
        Validators.required,
        FormCustomValidators.matchValidator('confirmPassword', true)
        ]),
      confirmPassword: new FormControl('', [
        Validators.required,
        FormCustomValidators.matchValidator('password')
      ])
    });
  }

  register() {
    if (this.registerFormControlGroup.valid) {
        this.request.username = this.username?.value;
        this.request.password = this.password?.value;
        this.request.displayName = this.displayName?.value;
    }

    this.authService.register(this.request).then((result) => {
      localStorage.setItem('user', JSON.stringify(ObjectGenerator.userFromToken(result.token)));
      this.router.navigate(['/login']);
    });
  }

  openLoginForm() {
    this.registerForm.close();
    this.dialog.open(LoginComponent);
  }

  cancel() {
    this.registerForm.close();
  }

  get username() {
    return this.registerFormControlGroup.get('username');
  }

  get password() {
    return this.registerFormControlGroup.get('password')!;
  }

  get displayName() {
    return this.registerFormControlGroup.get('displayName');
  }

  get confirmPassword() {
    return this.registerFormControlGroup.get('confirmPassword')!;
  }



}
