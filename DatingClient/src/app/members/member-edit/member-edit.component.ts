import { Component, HostListener, inject, ViewChild } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account-service.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotosEditComponent } from "../photos-edit/photos-edit.component";

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [FormsModule, TabsModule, PhotosEditComponent],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent {
  @ViewChild("editForm") editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event:any){
    if(this.editForm?.dirty)
    {
      $event.returnValue = true
    }
  }
  private memberService = inject(MemberService);
  private accountService = inject(AccountService);
  private toasterService = inject(ToastrService);

  member?: Member;
  ngOnInit(): void {
    this.loadMember()
  }

  loadMember() {
    const user = this.accountService.currentUser();
    if (user) {
      this.memberService.LoadMember(user.userName).subscribe({
        next: response => { this.member = response }
      })
    }
  }

  updateMember() {
    if (this.member) {
      this.memberService.UpdateMember(this.member).subscribe(
        {
          next: _=> {
            this.toasterService.success('Form Updated');
            this.editForm?.reset(this.member)
          },
          error: error=> this.toasterService.error(error)
        }
      )
    }
    else {
       this.toasterService.error('Form update failed');
    }
  }

  onMemberChange(event: Member)
  {
    this.member = event;
  }
}
