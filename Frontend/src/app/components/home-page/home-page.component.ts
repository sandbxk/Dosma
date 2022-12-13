import { Component, OnInit } from '@angular/core';
import {UserService} from "../../../services/user.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {RegisterComponent} from "../register/register.component";
import {LoginComponent} from "../login/login.component";

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  constructor(
    private userService: UserService,
    private router: Router,
    private matDialog: MatDialog
  ) { }

  isUserLoggedIn: boolean = false;

  ngOnInit(): void {
    this.isUserLoggedIn = this.userService.isLoggedIn();
  }

  login() {
    this.matDialog.open(LoginComponent);
  }

  register() {
    this.matDialog.open(RegisterComponent);
  }


}
