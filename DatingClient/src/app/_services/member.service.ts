import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, OnInit, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/Photo';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  httpClient = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);
  
  LoadMembers() {
    if(this.members().length > 0) return;
    
    this.httpClient.get<Member[]>(this.baseUrl + 'user').subscribe(
      {
        next:  response => this.members.set(response)
      }
    );
  }

  LoadMember(userName: any) {
    const member = this.members().find(x=> x.userName === userName);

    if(member) return of(member);

    return this.httpClient.get<Member>(this.baseUrl+'user/username/'+userName);
  }

  UpdateMember(member: Member) {
    return this.httpClient.put(this.baseUrl+'user', member).pipe(
      tap(()=>{
        this.members.update(
          members => members.map(m=>m.userName === member.userName ?  member : m));
      })
    );
  }

  SetMainPhoto(photo: Photo) {
    return this.httpClient.put(this.baseUrl + `user/set-main-photo/${photo.id}`,{}).pipe(
      tap(()=>{
        this.members.update(members=> members.map(m=>{
          if(m.photos.includes(photo)){
            m.photoUrl = photo.url;           
          }
          return m;
        })
      )})
    );
  }
  
  DeletePhoto(photo: Photo){
    return this.httpClient.delete(this.baseUrl+'user/delete-photo/'+ photo.id).pipe(
      tap(()=>{
        this.members.update(
          members => members.map(m=>{
            if(m.photos.includes(photo))
            {
              m.photos = m.photos.filter(mp=> mp.id !== photo.id)
            }
            return m;
          }));
      })
    );
  }
}
