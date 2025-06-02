import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes.service';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { Member } from '../_models/member';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [ButtonsModule, FormsModule, MemberCardComponent, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit, OnDestroy {
  likeService = inject(LikesService);
  predicate = 'liked';

  ngOnInit(): void {
    this.LoadMembers();
  }

  LoadMembers() {
    this.likeService.getLikes();
  }

  getTitle() {
    switch (this.likeService.likesParams.predicate) {
      case 'liked': return 'Who I Like';
      case 'likedBy': return 'Who Likes Me';
      default: return 'Mutual';
    }
  }

  pageChange(event: any) {
    if(event.page === this.likeService.likesParams.pageNumber) return;
    this.likeService.likesParams.pageNumber = event.page;
    this.LoadMembers();
  }

   ngOnDestroy(): void {
    this.likeService.paginatedResult.set(null);
  }

}
