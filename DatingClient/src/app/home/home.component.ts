import { Component } from '@angular/core';
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  registerMode: boolean = false;

  register(){
    this.registerMode = !this.registerMode;
  }

  learnMore() {
    throw new Error('Method not implemented.');
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
