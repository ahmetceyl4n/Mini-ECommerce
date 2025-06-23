import { Component, OnInit } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-baskets',
  standalone: false,
  templateUrl: './baskets.component.html',
  styleUrl: './baskets.component.scss'
})
export class BasketsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService) { 
    super(spinner); // Call the constructor of the base class
  }
  ngOnInit(): void {
    // Initialize the component
    this.showSpinner(SpinnerType.SquareJellyBox); // Show the loading spinner
  }
}