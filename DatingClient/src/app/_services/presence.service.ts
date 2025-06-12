import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import signalR, { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../_models/user';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
 hubsUrl = environment.hubsUrl;
 private toasterService = inject(ToastrService);
 private hubConnection?: HubConnection;
 private router = inject(Router);
 onlineUsers = signal<string[]>([]);
 
 createHubConnection(user:User){
  this.hubConnection =  new HubConnectionBuilder()
    .withUrl(this.hubsUrl + 'presence', {
      accessTokenFactory: () => user.token })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start().catch(error => console.log("Error establishing connection :", error));

    this.hubConnection.on('UserIsOnline', userName => {
      this.onlineUsers.update(users => [...users, userName])
    });

    this.hubConnection.on('UserIsOffline', userName => {
      this.onlineUsers.update(users => users.filter(x=> x!== userName));
    });

    this.hubConnection.on('GetOnlineUsers', userNames => {
      this.onlineUsers.set(userNames);
    })

    this.hubConnection.on('NewMessageReceived', ({userName, knownAs}) => {
      this.toasterService.info(knownAs + ' has sent you a new message! Click me to see it')
      .onTap
      .pipe(take(1))
      .subscribe(() => this.router.navigateByUrl('/members/' + userName + '?tab=Messages'))
    })

   window.addEventListener('beforeunload', () => {
     console.log('Page is being refreshed - stopping SignalR connection.');
     this.hubConnection?.stop();
   });

 }

 stopHubConnection(){
  if(this.hubConnection?.state === HubConnectionState.Connected){
    this.hubConnection.stop().catch(error => console.log(error));
  }
 }
}
