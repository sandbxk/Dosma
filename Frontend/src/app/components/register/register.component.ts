import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { RegisterRequest, User } from 'src/app/interfaces/User';
import { ObjectGenerator } from 'src/app/util/ObjectGenerator';
import { AuthenticationService } from 'src/services/authentication.service';
import { LoginComponent } from '../login/login.component';

@Component({
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {

  constructor(
    private authService : AuthenticationService,
    private router: Router,
    public registerForm: MatDialogRef<RegisterComponent>,
    private dialog: MatDialog
    ) { }

  request : RegisterRequest = new RegisterRequest();

  confirm_password : string = '';

  register() {
    if (this.request.username == '' || this.request.password == '') {
      alert('Please fill in all required (*) fields');
      return;
    }

    if (this.request.password != this.confirm_password) {
      alert('passwords do not match');
      return;
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

  ngOnInit(): void {

  }
}
