import { Component, inject, OnInit } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { ActivatedRoute } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { CarouselModule  } from 'ngx-bootstrap/carousel'
@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, CarouselModule ],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MemberService);
  private route =inject(ActivatedRoute);
  member? : Member;
  activeSlideIndex = 0;
  ngOnInit(): void {
    this.loadMember()
  }

  loadMember() {
    const userName = this.route.snapshot.paramMap.get('userName');
    if(!userName) return;

    this.memberService.LoadMember(userName).subscribe({
      next: response =>{ this.member = response}
    })
  }
}