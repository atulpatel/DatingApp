import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() weatherForecastsFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) {}

  ngOnInit() {}

  register() {
    this.authService.register(this.model).subscribe(
      () => {
        this.alertify.success('Registered successfully');
        console.log('Registered successfully');
      },
      error => {
        this.alertify.success(error);
        console.log(error);
      }
    );
    console.log(this.model);
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.success('cancelled');
    console.log('cancelled');
  }
}
