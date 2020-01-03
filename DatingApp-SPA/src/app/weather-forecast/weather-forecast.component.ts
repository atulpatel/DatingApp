import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrls: ['./weather-forecast.component.css']
})
export class WeatherForecastComponent implements OnInit {
  forecast: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getWeatherForecast();
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
}
