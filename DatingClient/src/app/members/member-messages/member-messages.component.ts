import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Message } from '../../_models/message';
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
  messages = input.required<Message[]>();
  messageContent = '';
  updateMessage = output<Message>();

  sendMessage() {
    this.messageService.sendMessage(this.userName(), this.messageContent).subscribe({
      next: (message) => {
        this.updateMessage.emit(message);
        this.messageForm?.reset();
      }
    });
  }

}
