import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner'; // Import NgxSpinnerService for loading spinner
import { BaseComponent, SpinnerType } from '../../../base/base.component';

@Component({
  selector: 'app-order',
  standalone: false,
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService) { 
    super(spinner); // Call the constructor of the base class
  }
  ngOnInit(): void {
    // Initialize the component
    this.showSpinner(SpinnerType.SquareJellyBox); // Show the loading spinner
  }
}
