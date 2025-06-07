import { Component, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs'
import { CarouselModule  } from 'ngx-bootstrap/carousel'
import { DatePipe } from '@angular/common';
import { TimeagoPipe } from '../../_pipes/timeago.pipe';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, CarouselModule, DatePipe, TimeagoPipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs' ,{ static: true } ) memberTabs?: TabsetComponent;
  private memberService = inject(MemberService);
  private route =inject(ActivatedRoute);
  member: Member = {} as Member;
  activeSlideIndex = 0;
  messages: Message[] = [];
  activeTab?: TabDirective;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
      }
    });

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

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.loadMessages();
    }
  }

  loadMessages() {
    if (!this.member) return;
    this.memberService.getMessageThread(this.member.userName).subscribe({
      next: messages => this.messages = messages,
      error: error => console.error('Error loading messages:', error)
    });
  }


  loadMember() {
    const userName = this.route.snapshot.paramMap.get('userName');
    if(!userName) return;

    this.memberService.LoadMember(userName).subscribe({
      next: response =>{ this.member = response}
    })
  }

  onMessageUpdate($event: Message) {
    this.messages.push($event);
  }

}