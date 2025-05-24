import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TitleCasePipe } from '@angular/common';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AccountService } from '../_services/account-service.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

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
  model: any = {};
  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => { this.routerService.navigateByUrl('/members') },
      error: error => { console.log(error)}      
    })
  }

  logout() {
    this.accountService.logout();
    this.routerService.navigateByUrl('');
  }

}
