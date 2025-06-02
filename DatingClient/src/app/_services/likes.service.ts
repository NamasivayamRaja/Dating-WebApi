import { inject, Injectable, signal, Signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/paginated-result';
import { LikesParams } from '../_models/likes-params';
import { setPaginatedHeaderparams, setPaginatedResponse } from './_pagination-helper';

@Injectable({
  providedIn: 'root'
})
export class LikesService 
{
  httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl + 'likes/';
  likeIds= signal<number[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  likesParams = new LikesParams();

  toggleLike(targetId:number){
    return this.httpClient.post(this.baseUrl + targetId , {});
  }

  getLikes(){
    var params = setPaginatedHeaderparams(this.likesParams.pageSize, this.likesParams.pageNumber);
    params = params.append('predicate', this.likesParams.predicate);
    return this.httpClient.get<Member[]>(this.baseUrl, { observe: 'response', params }).subscribe
    ({
      next: (response) => {
        setPaginatedResponse(response, this.paginatedResult);
      },
      error: (error) => {
        console.error('Error loading likes:', error);
      } 
    });
  }

  getLikeIds() {
    return this.httpClient.get<number[]>(this.baseUrl + 'list').subscribe({
      next: (ids) => {
        this.likeIds.set(ids);
      }
    });
  }
}

