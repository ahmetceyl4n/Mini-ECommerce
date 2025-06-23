import { Component, OnInit } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService) { 
    super(spinner); // Call the constructor of the base class
  }
  ngOnInit(): void {
    // Initialize the component
    this.showSpinner(SpinnerType.SquareJellyBox); // Show the loading spinner
  }
}