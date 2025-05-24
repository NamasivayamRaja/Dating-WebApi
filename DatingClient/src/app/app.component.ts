import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./navbar/navbar.component";
import { AccountService } from './_services/account-service.service';
import { User } from './_models/user';
import { HomeComponent } from "./home/home.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor, NavbarComponent, HomeComponent],
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
    this.getUsers();
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

  private getUsers() {
    this.httpClient.get('https://localhost:7118/api/user').subscribe(
      response => { this.users = response; },
      error => { console.log(error); },
      () => { console.log('Request Completed'); }
    );
  }
}
