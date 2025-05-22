import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  httpClient = inject(HttpClient);
  title = 'DatingClient';
  users: any;

  //constructor(private httpClient: HttpClient){}

  ngOnInit(){
    this.httpClient.get('https://localhost:7118/api/user').subscribe(
        response => { this.users = response },
        error => { console.log(error) },
        () => { console.log('Request Completed'); }
    );
  }
}
