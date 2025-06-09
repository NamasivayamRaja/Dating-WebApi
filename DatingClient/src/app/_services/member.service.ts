import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable, model, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of } from 'rxjs';
import { Photo } from '../_models/Photo';
import { PaginatedResult } from '../_models/paginated-result';
import { UserParams } from '../_models/user-params';
import { AccountService } from './account-service.service';
import { setPaginatedHeaderparams, setPaginatedResponse } from './_pagination-helper';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private httpClient = inject(HttpClient);
  private accountService = inject(AccountService);
  private baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memoryCache = new Map<string, HttpResponse<Member[]>>();
  user = this.accountService.currentUser();
  userParams = model<UserParams>(new UserParams(this.user));

  ResetUserParams() {
    this.userParams.set(new UserParams(this.user));
  }

  getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + 'messages/thread/' + userName);
  }


  LoadMembers() {
    let cachedMembersResponse = this.memoryCache.get(Object.values(this.userParams()).join('-'));

    if (cachedMembersResponse) {
      return setPaginatedResponse(cachedMembersResponse, this.paginatedResult); 
    }

    let params = setPaginatedHeaderparams(this.userParams().pageSize, this.userParams().pageNumber);
    

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    this.httpClient
      .get<Member[]>(this.baseUrl + 'user', { observe: 'response', params })
      .subscribe({
        next: response => {
          setPaginatedResponse(response, this.paginatedResult);
          this.memoryCache.set(Object.values(this.userParams()).join('-'), response);
        }
      });
  }

  LoadMember(userName: any) {
    const member = this.paginatedResult()?.items?.find(x=> x.userName === userName);

    if(member) return of(member);

    return this.httpClient.get<Member>(this.baseUrl+'user/username/'+userName);
  }

  UpdateMember(member: Member) {
    return this.httpClient.put(this.baseUrl+'user', member).pipe(
      // tap(()=>{
      //   this.members.update(
      //     members => members.map(m=>m.userName === member.userName ?  member : m));
      // })
    );
  }

  SetMainPhoto(photo: Photo) {
    return this.httpClient.put(this.baseUrl + `user/set-main-photo/${photo.id}`,{}).pipe(
      // tap(()=>{
      //   this.members.update(members=> members.map(m=>{
      //     if(m.photos.includes(photo)){
      //       m.photoUrl = photo.url;           
      //     }
      //     return m;
      //   })
      // )})
    );
  }  
  
  DeletePhoto(photo: Photo){
    return this.httpClient.delete(this.baseUrl+'user/delete-photo/'+ photo.id).pipe(
      // tap(()=>{
      //   this.RemovePhotoFromState(photo);
      // })
    );
  }


  RemovePhotoFromState(photo: Photo) {
    // this.members.update(
    //   members => members.map(m => {
    //     if (m.photos.includes(photo)) {
    //       m.photos = m.photos.filter(mp => mp.id !== photo.id)
    //     }
    //     return m;
    //   }));
  }
}
