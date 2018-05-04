import { Component, Inject, OnInit} from '@angular/core';
import { Http, Response } from '@angular/http';
import { HubConnection, IHubConnectionOptions } from '@aspnet/signalr';
import 'rxjs/add/operator/map';

@Component({
    selector: 'lightcontroller',
    templateUrl: './lightController.component.html',
    styleUrls: ['./lightController.component.css']
})
export class LightControllerComponent implements OnInit{
    public isLightOn: string;
    public test: string;
    public _hubConnection: HubConnection;
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {   
    }

    ngOnInit() {
        this._hubConnection = new HubConnection('http://localhost:51691/message');
        this._hubConnection.on('Send', (data: string) => {
            this.isLightOn = data;
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started')
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });

        console.log(this._hubConnection);
    }

    public incrementCounter() {
        //this.http.get(this.baseUrl + 'api/SampleData/CheckStatus').subscribe();
        //var test = this.http.get(this.baseUrl + 'api/SampleData/GetBulbStatus').subscribe(result => {
        //    this.test = result.json();
        //}, error => console.error(error));
       
    }

    public lightUp() {
        this._hubConnection.invoke('ChangeLightState', true);
        this._hubConnection.invoke('CheckStatusOfLight');
        //this.isLightOn = !this.isLightOn;
    }
}
