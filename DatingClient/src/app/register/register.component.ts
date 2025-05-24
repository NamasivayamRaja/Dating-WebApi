import { Component, inject, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account-service.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  model: any = {};
  // old @Output
  //@Output() cancelRegister = new EventEmitter<boolean>();
  // using output()
  cancelRegister = output<boolean>();
  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: userResponse => { console.log(userResponse); this.cancel(); },
      error: err => { console.log(err) }
    }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
