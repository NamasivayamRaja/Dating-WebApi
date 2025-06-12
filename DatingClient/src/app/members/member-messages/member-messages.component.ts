import { Component, inject, input, ViewChild } from '@angular/core';
import { TimeagoPipe } from '../../_pipes/timeago.pipe';
import { FormsModule, NgForm } from '@angular/forms';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoPipe, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent {
  @ViewChild('messageForm') messageForm? : NgForm;
  messageService = inject(MessageService);
  userName = input.required<string>();
  messageContent = '';
 
  sendMessage() {
    this.messageService.sendMessage(this.userName(), this.messageContent).then(()=>{
      this.messageForm?.reset();
    });
  }

}
