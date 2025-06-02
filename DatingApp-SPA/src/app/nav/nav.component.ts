import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) {}

  ngOnInit() {}
  login() {
    console.log(this.model);
    this.authService.login(this.model).subscribe(
      next => {
        console.log(next);
        this.alertify.success('Login successfully')
        console.log('Login successfully');
      },
      error => {
        console.log(error);
        this.alertify.success(error);
        // console.log('Failed to login');
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout(){
    localStorage.removeItem('token');
    console.log('Logged out');
    this.alertify.message('Logged out');
  }
}
