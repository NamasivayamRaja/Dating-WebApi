import { NgIf } from '@angular/common';
import { Component, input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-textinput',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './textinput.component.html',
  styleUrl: './textinput.component.css'
})
export class TextinputComponent implements ControlValueAccessor{
  label = input<string>('');
  type = input<string>('text');

  constructor(@Self() public ngControl : NgControl){
    this.ngControl.valueAccessor =  this;
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
