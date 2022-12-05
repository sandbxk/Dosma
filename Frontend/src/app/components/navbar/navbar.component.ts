import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  image = '../assets/Logo.png';

  constructor(private dialog: MatDialog) { }

  openLoginForm() : void
  {
    this.dialog.open(LoginComponent);
  }

  ngOnInit(): void {
  }

}
