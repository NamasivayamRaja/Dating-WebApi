import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { Component, inject, input, OnInit, output } from '@angular/core';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account-service.service';
import { Member } from '../../_models/member';
import { environment } from '../../../environments/environment';
import { MemberService } from '../../_services/member.service';
import { ToastrService } from 'ngx-toastr';
import { Photo } from '../../_models/Photo';

@Component({
  selector: 'app-photos-edit',
  standalone: true,
  imports: [NgFor, NgClass, NgStyle, NgIf, FileUploadModule,DecimalPipe],
  templateUrl: './photos-edit.component.html',
  styleUrl: './photos-edit.component.css'
})
export class PhotosEditComponent implements OnInit {
  private accountService = inject(AccountService)
  private memberService = inject(MemberService);
  private toasterService = inject(ToastrService);

  member = input.required<Member>();
  uploader?: FileUploader;
  hasBaseDropZoneOver: boolean = false;
  baseUrl= environment.apiUrl;
  memberChange = output<Member>();

  ngOnInit(): void {
    this.intializeUploader();
  }

  intializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials= false;
    } 

    this.uploader.onSuccessItem = (item, response, status, header) =>{
      const photo = JSON.parse(response);
      const updatedMember = {...this.member()};
      updatedMember.photos.push(photo);
      if (updatedMember.photos.length === 1 && photo.isMain) {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user)
        }
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          if (p.isMain)
            p.isMain = false;
          if (p.id === photo.id)
            p.isMain = true;
        });
        this.memberService.members.update(members => members.map(m => {
          if (m.userName == user?.userName && m.photos.length == 0) {
            m.photos = [photo];
            m.photoUrl = photo.url;
          }
          return m;
        }));
      }
      this.memberChange.emit(updatedMember);
    };
  }


  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  SetMainPhoto(photo:Photo) {
    this.memberService.SetMainPhoto(photo).subscribe({
      next: response => {
        this.toasterService.success("Main Photo Updated");
        const user = this.accountService.currentUser();
        if(user){
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user)
        }

        const updateMember = {...this.member()};
        updateMember.photoUrl = photo.url;
        updateMember.photos.forEach(p=>{
          if(p.isMain) 
            p.isMain = false;
          if(p.id === photo.id)
            p.isMain = true;
        })
        this.memberChange.emit(updateMember);
      },
      error: err => {
        this.toasterService.error(err, "Faied");
      }
    })
  }


  DeletePhoto(photo:Photo) {
    const userName = this.accountService.currentUser()?.userName;
    if (userName)
      this.memberService.DeletePhoto(photo).subscribe({
        next: response => {
          this.toasterService.success("Photo deleted");
          const updateMember = {...this.member()};
          updateMember.photos = updateMember.photos.filter(p => p.id !== photo.id);
          this.memberChange.emit(updateMember);
          this.memberService.RemovePhotoFromState(photo);
        },
        error: err => {
          this.toasterService.error(err, "Photo deletion failed");
        }
      })
  }

}