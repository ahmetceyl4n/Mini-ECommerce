import { Component, OnInit } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-customer',
  standalone: false,
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.scss'
})
export class CustomerComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService) { 
    super(spinner); // Call the constructor of the base class
  }
  ngOnInit(): void {
    // Initialize the component
    
  }
}