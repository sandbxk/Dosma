import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {  Router } from '@angular/router';
import { UserService } from 'src/services/user.service';
import { LoginComponent } from '../login/login.component';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  image = '../assets/Logo.png';

  constructor(
    private dialog: MatDialog,
    private router: Router,
    public user: UserService
  ) { }

  openLoginForm() : void
  {
    this.dialog.open(LoginComponent);
  }

  openRegisterForm() : void
  {
    this.dialog.open(RegisterComponent);
  }

  goHome() : void
  {
    this.router.navigate(['dashboard']);
  }

  logoutCurrentUser() {
    localStorage.removeItem('user');
    this.router.navigate(['home']);
  }

  ngOnInit(): void {
  }

}
