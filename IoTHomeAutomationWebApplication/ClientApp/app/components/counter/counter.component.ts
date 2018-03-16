import { Component } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
    selector: 'counter',
    templateUrl: './counter.component.html'
})
export class CounterComponent {
    public currentCount = 0;

    public incrementCounter() {
        let connection = new HubConnection('http://localhost:51691/message');

         //client methods that can be invoke by server
        connection.on('send', data => {
            console.log(data);
        });

        //invoking server methods
        connection.start()
            .then(() => { console.log('Connected!!!!!!!!'), connection.invoke('send', "Kurwa mac ile mamy stac") } );
    }
}
