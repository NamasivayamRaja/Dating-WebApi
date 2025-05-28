import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./navbar/navbar.component";
import { AccountService } from './_services/account-service.service';
import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private httpClient = inject(HttpClient);
  accountService = inject(AccountService);
  private routerService = inject(Router);
  title = 'DatingClient';
  users: any;

  //constructor(private httpClient: HttpClient){}

  ngOnInit(){
    this.setUser();
  }

  private setUser(){
    const userString =  localStorage.getItem('user');
    if(userString)
    {
      const userp = JSON.parse(userString)
      this.accountService.currentUser.set(userp)
      this.routerService.navigateByUrl('/members');
    }
  }
}
