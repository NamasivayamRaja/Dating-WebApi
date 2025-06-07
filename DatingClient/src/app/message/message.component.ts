import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { TimeagoPipe } from '../_pipes/timeago.pipe';
import { Message } from '../_models/message';
import { RouterLink } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [ButtonsModule, FormsModule, RouterLink , TimeagoPipe, PaginationModule],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
messageService = inject(MessageService);
pageNumber = 1;
pageSize = 5;
container = 'Outbox';
isInbox = this.container == 'Inbox';
  ngOnInit(): void {
    this.LoadMessages();
  }

  LoadMessages() {
    this.messageService.getPages(this.pageNumber, this.pageSize, this.container)
  }

  getRoute(message: Message){
    if(this.container === 'Inbox') {
      return `/members/${message.senderUserName}`;
    }
    else return `/members/${message.recipientUserName}`;
  }

  pageChange(event: any) {
    if (event.page === this.pageNumber) return; 
    this.pageNumber = event.page;
    this.LoadMessages();
  }
  
  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => {
        this.messageService.paginatedResult.update(messages => {
          if (messages && messages.items) {
            messages.items.splice(messages.items.findIndex(m => m.id === id), 1);
          }
          return messages;
        })
      }
    });
  }
}
