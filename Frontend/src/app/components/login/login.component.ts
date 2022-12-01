import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginRequest, User } from 'src/app/interfaces/User';
import { ObjectGenerator } from 'src/app/util/ObjectGenerator';
import { AuthenticationService } from 'src/services/authentication.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginRequest : LoginRequest = new LoginRequest();

  login() {
    console.log('login called');

    this.authService.login(this.loginRequest).then((result) => {
      localStorage.setItem('user', JSON.stringify(ObjectGenerator.userFromToken(result.token)));
      this.router.navigate(['/dashboard']);
    });
  }

  constructor(private authService: AuthenticationService, private router : Router) {}

  ngOnInit(): void {

    let user : User | null = JSON.parse(localStorage.getItem('user') || '{}');

    if (user) {
      console.log('token found in local storage');
      // TODO: check if token is valid
      // navigate to dashboard
    }
  }
}
