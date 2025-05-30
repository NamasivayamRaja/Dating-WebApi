import { NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule, BsDaterangepickerConfig } from 'ngx-bootstrap/datepicker';
@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, BsDatepickerModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css'
})
export class DatePickerComponent implements ControlValueAccessor {
  label = input<string>('');
  maxDate = input<Date>();
  bsDaterangepickerConfig?: Partial<BsDaterangepickerConfig> 

  constructor(@Self() private ngControl : NgControl){
    this.ngControl.valueAccessor = this;
    this.bsDaterangepickerConfig  = {
      containerClass: 'theme-dark-blue',
      dateInputFormat: 'DD MMMM YYYY',
      maxDate: this.maxDate()
    }
  }
  
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }

  get control() : FormControl{
    return this.ngControl.control as FormControl;
  }

}
