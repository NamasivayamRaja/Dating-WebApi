import { Component, inject, OnInit } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/user-params';
import { FormsModule } from '@angular/forms';
import { Gender } from '../../_enums/gender';
import { AccountService } from '../../_services/account-service.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
@Component({
  selector: 'app-members-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css'
})
export class MembersListComponent implements OnInit {
  accountService = inject(AccountService);
  memberService = inject(MemberService);
  genders = Object.keys(Gender).filter(key => isNaN(Number(key)));
  ngOnInit(): void {
    this.loadMembers()
  }

  public loadMembers() {
    this.memberService.LoadMembers();
  }

  pageChange(event: any) {
    if (event.page === this.memberService.userParams().pageNumber) return;
    this.memberService.userParams().pageNumber = event.page;
    this.loadMembers();
  }

  resetFilters() {
    this.memberService.ResetUserParams();
    this.loadMembers();
  }
}