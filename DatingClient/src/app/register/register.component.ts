import { Component, inject, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  private routeService = inject(Router)
  model: any = {};
  // old @Output
  //@Output() cancelRegister = new EventEmitter<boolean>();
  // using output()
  cancelRegister = output<boolean>();
  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: _ => { this.routeService.navigateByUrl('/members'); },
      error: err => { console.log(err) }
    }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
