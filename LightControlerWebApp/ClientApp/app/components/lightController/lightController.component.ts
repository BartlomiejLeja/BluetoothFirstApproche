import { Component, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/map';

@Component({
    selector: 'lightcontroller',
    templateUrl: './lightController.component.html',
    styleUrls: ['./lightController.component.css']
})
export class LightControllerComponent {
    public isLightOn: boolean;
    public test: any;
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/ConnectToSignalR').subscribe();    
        this.isLightOn = false;
    }

    public incrementCounter() {
        this.http.get(this.baseUrl + 'api/SampleData/CheckStatus').subscribe();
        var test = this.http.get(this.baseUrl + 'api/SampleData/GetBulbStatus').subscribe(result => {
            this.test = result.json();
        }, error => console.error(error));
    }

    public lightUp() {
        this.http.get(this.baseUrl + 'api/SampleData/LightUp').subscribe();
       
        this.isLightOn = !this.isLightOn;
    }
}
