import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/paginated-result';
import { Message } from '../_models/message';
import { HttpClient } from '@angular/common/http';
import { setPaginatedHeaderparams, setPaginatedResponse } from './_pagination-helper';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../_models/user';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl = environment.apiUrl
hubUrl = environment.hubsUrl;
private httpClient = inject(HttpClient);
hubConnection? : HubConnection;
paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
messageThread = signal<Message[]>([]);
  createHubConnection(user: User, otherUserName: string){
    this.hubConnection = new HubConnectionBuilder()
                        .withUrl(this.hubUrl + 'message?user=' + otherUserName, {
                          accessTokenFactory: () => user.token
                        })
                        .withAutomaticReconnect()
                        .build();

    this.hubConnection.start()
    .catch(error=> console.log("Im from message thread",error));  
    
    this.hubConnection.on('ReceiveMessageThread', messages =>{
      this.messageThread.set(messages);
    });

    this.hubConnection.on('NewMessage', message =>{
      this.messageThread.update(messages => [...messages, message])
    })

    this.hubConnection.on('UpdatedGroup', (group:Group) => {
      if(group.connections.some(x=>x.userName === otherUserName)){
        this.messageThread.update(messages => {
          messages.forEach(message =>{
            if(!message.dateRead){
              message.dateRead = new Date(Date.now());
            }
          })
          return messages;
        })
      }
    })
  }

  stopHubConnection(){
    if(this.hubConnection?.state === HubConnectionState.Connected){
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }

  getPages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginatedHeaderparams(pageSize, pageNumber);
    params = params.append('Container', container);
    return this.httpClient.get<Message[]>(this.baseUrl + 'messages', { observe: 'response', params })
      .subscribe({
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
        }
      });
  }

  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', {recipientUserName: username, content});
  }

  deleteMessage(id: number){
    return this.httpClient.delete(this.baseUrl + 'messages/' + id);
  }
}
