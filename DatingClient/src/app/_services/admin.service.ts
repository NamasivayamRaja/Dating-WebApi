import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
 baseUrl = environment.apiUrl;
 private httpClient = inject(HttpClient);

 getUserWithRoles(){
  return this.httpClient.get<User[]>(this.baseUrl+'admin/users-with-roles')
 }

 updateUserRoles(userName: string, roles: string[])
 {
  return this.httpClient.post<string[]>(
    this.baseUrl+'admin/edit-roles/' + userName + '?roles=' + roles, {}
  )
 }
}
