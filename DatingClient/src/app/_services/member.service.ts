import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  httpClient = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  LoadMembers() {
    return this.httpClient.get<Member[]>(this.baseUrl + 'user');
  }

  LoadMember(userName: any) {
    return this.httpClient.get<Member>(this.baseUrl+'user/username/'+userName);
  }
}
