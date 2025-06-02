import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';
import { ToastrService } from 'ngx-toastr';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink, NgClass],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  private toasterService = inject(ToastrService);
  private likeService = inject(LikesService)
  member = input.required<Member>()
  hasLike = computed(() => this.likeService.likeIds().includes(this.member().id));

  toogleLikes() {
    this.likeService.toggleLike(this.member().id).subscribe({
      next: () => {
        if (this.hasLike()) {
          this.likeService.likeIds.update(ids => ids.filter(id => id !== this.member().id));
        }
        else {
          this.likeService.likeIds.update(ids => [...ids, this.member().id]);
        }
      },
      error: (error) => {
        this.toasterService.error(error.error, "Error");
      }
    });
  }

}
