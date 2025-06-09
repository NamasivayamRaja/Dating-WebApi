import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  private adminService  = inject(AdminService);
  private bsModalService =inject(BsModalService);
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  users: User[] = [];
  ngOnInit(): void {
    this.LoadUsers()
  }

  openRolesModel(user: User){
    const intialState : ModalOptions = {
      class: 'modals-lg',
      initialState:{
        title: 'User Roles',
        userName: user.userName,
        selectedRoles: [...user.roles!],
        availableRoles: ['Admin', 'Moderator', 'Member'],
        users: this.users,
        rolesUpdated: false
      }
    }
    this.bsModalRef = this.bsModalService.show(RolesModalComponent, intialState);
    this.bsModalRef.onHide?.subscribe({
      next: () =>
        {
          if(this.bsModalRef.content && this.bsModalRef.content.rolesUpdated){
            const selectedRoles = this.bsModalRef.content.selectedRoles;
            this.adminService.updateUserRoles(user.userName, selectedRoles).subscribe({
              next: (roles) => {
                user.roles = roles
              }
            });
          }
        }
    })
  }

  LoadUsers() {
    this.adminService.getUserWithRoles().subscribe({
      next: (user) => this.users = user
    })
  }

}
