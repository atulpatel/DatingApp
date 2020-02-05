import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  forecast: any;
  isRegisterMode = false;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getWeatherForecast();
  }

  toggleIsRegisterMode(){
    this.isRegisterMode = true;
  }
  getWeatherForecast() {
    this.http.get('http://localhost:5000/WeatherForecast').subscribe(
      response => {
        this.forecast = response;
      },
      error => {
        console.log(error);
      }
    );
  }
  cancelRegisterMode(cancelRegisterMode: boolean){
    this.isRegisterMode = cancelRegisterMode; 
  }
}
