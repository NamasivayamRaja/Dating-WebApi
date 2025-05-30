import { Component, inject, OnInit, output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account-service.service';
import { Router } from '@angular/router';
import { TextinputComponent } from "../_forms/textinput/textinput.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextinputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  private routeService = inject(Router)
  private formBuilder = inject(FormBuilder);
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string[] = [];
  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(new Date().getFullYear() - 18);    
  }


  initializeForm() {
    this.registerForm = this.formBuilder.group({
      gender: ['male'],      
      userName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      knownAs: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],                  
      password: ['',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(8)
        ]],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    });
    
    this.registerForm.controls['password'].valueChanges.subscribe({
        next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity() 
    });
  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { isMatching: true };
    }
  } 

  cancelRegister = output<boolean>();
  
  register() {
    this.validationErrors = [];
    const dateOfBirth = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.patchValue( {dateOfBirth: dateOfBirth })
    this.accountService.register(this.registerForm.value).subscribe({
      next: _ => { this.routeService.navigateByUrl('/members'); },
      error: err => { 
        if(err){
          this.validationErrors= err;
        }
       }
    });
  }

  private getDateOnly(date : Date) : string | undefined {
    if(!date)
      return;
    return new Date(date).toISOString().slice(0,10);
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

}
