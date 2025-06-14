import { AfterViewChecked, Component, inject, input, ViewChild } from '@angular/core';
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
export class MemberMessagesComponent implements AfterViewChecked {
  @ViewChild('messageForm') messageForm? : NgForm;
  @ViewChild('scrollMe') scrollContainer?: any
  messageService = inject(MessageService);
  userName = input.required<string>();
  messageContent = '';
 
  sendMessage() {
    this.messageService.sendMessage(this.userName(), this.messageContent).then(()=>{
      this.messageForm?.reset();
      this.scrollToBottom();
    });
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom(){
    if(this.scrollContainer){
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }

}
