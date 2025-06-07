import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/paginated-result';
import { Message } from '../_models/message';
import { HttpClient } from '@angular/common/http';
import { setPaginatedHeaderparams, setPaginatedResponse } from './_pagination-helper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl = environment.apiUrl
private httpClient = inject(HttpClient);
paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

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

  sendMessage(username: string, content: string) {
    return this.httpClient.post<Message>(this.baseUrl + 'messages', { recipientUsername: username, content });
  }

  deleteMessage(id: number){
    return this.httpClient.delete(this.baseUrl + 'messages/' + id);
  }
}
