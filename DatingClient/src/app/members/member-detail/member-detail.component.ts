import { Component, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs'
import { CarouselModule  } from 'ngx-bootstrap/carousel'
import { DatePipe } from '@angular/common';
import { TimeagoPipe } from '../../_pipes/timeago.pipe';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account-service.service';
import { MessageService } from '../../_services/message.service';
import { HubConnectionState } from '@microsoft/signalr';
@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, CarouselModule, DatePipe, TimeagoPipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs' ,{ static: true } ) memberTabs?: TabsetComponent;
  presenceService = inject(PresenceService);  
  private memberService = inject(MemberService);
  private router = inject(Router);
  private route =inject(ActivatedRoute);
  private accountService = inject(AccountService);
  private messageService = inject(MessageService);

  member: Member = {} as Member;
  activeSlideIndex = 0;
  activeTab?: TabDirective;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
      }
    });

    this.route.paramMap.subscribe({
      next: _=> this.onRouteParamsChange()
    })

    this.route.queryParamMap.subscribe(params => {
      const tabHeading = params.get('tab');
      if (tabHeading) {
        this.selectTab(tabHeading);
      } 
    });
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const tab = this.memberTabs.tabs.find(t => t.heading === heading);
      if (tab) {
        tab.active = true;
      }
    }
  }

  onRouteParamsChange() {
    const user = this.accountService.currentUser();

    if(!user) return;

    if(this.messageService.hubConnection?.state === HubConnectionState.Connected 
      && this.activeTab?.heading === "Messages"){
        this.messageService.hubConnection.stop().then(()=>{
          this.messageService.createHubConnection(user, this.member.userName);
        })
      }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this.router.navigate([], { 
      relativeTo: this.route,
      queryParams: {tab: this.activeTab.heading},
      queryParamsHandling:'merge'
    })
    if (this.activeTab.heading === 'Messages' && this.member) {
      const user = this.accountService.currentUser();
      if(!user) return;
      this.messageService.createHubConnection(user, this.member.userName)
    }
    else{
      this.messageService.stopHubConnection();
    }
  }


  loadMember() {
    const userName = this.route.snapshot.paramMap.get('userName');
    if(!userName) return;

    this.memberService.LoadMember(userName).subscribe({
      next: response =>{ this.member = response}
    })
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}