import { Component, OnInit } from '@angular/core';
import { LoginRequest } from 'src/app/interfaces/User';
import { AuthenticationService } from 'src/services/authentication.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  username: string = '';
  password: string = '';

  login() {
    console.log('login called');
    let loginRequest : LoginRequest = {
      username: this.username,
      password: this.password
    };

    this.authService.login(loginRequest).then((result) => {
      console.log(result);
    });
  }

  constructor(private authService: AuthenticationService) { }

  ngOnInit(): void {
  }

}
