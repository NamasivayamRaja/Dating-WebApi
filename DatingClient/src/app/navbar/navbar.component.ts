import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TitleCasePipe } from '@angular/common';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AccountService } from '../_services/account-service.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, TitleCasePipe,RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  accountService = inject(AccountService);
  private routerService =  inject(Router);
  private toasterService = inject(ToastrService);
  model: any = {};
  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => { this.routerService.navigateByUrl('/members') },
      error: error => { this.toasterService.error(error)}      
    })
  }

  logout() {
    this.accountService.logout();
    this.routerService.navigateByUrl('');
  }

}
