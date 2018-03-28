import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    public forecasts: WeatherForecast[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        
        //http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
        //    this.forecasts = result.json() as WeatherForecast[];
        //}, error => console.error(error));
        http.get(baseUrl + 'api/SampleData/ConnectToSignalR').subscribe();
    }

    public incrementCounter() {  
        this.http.get(this.baseUrl + 'api/SampleData/InvokeSendMethod').subscribe();
    }

    public lightUp() {
        this.http.get(this.baseUrl + 'api/SampleData/LightUp').subscribe();
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
