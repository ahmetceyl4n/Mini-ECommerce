import { Component } from '@angular/core';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';

export class BaseComponent {
  constructor(private spinner: NgxSpinnerService) {}
    showSpinner(spinnerNameType : SpinnerType){
      this.spinner.show(spinnerNameType); // Show the spinner with a specific animation

      setTimeout(() => {this.hideSpinner(spinnerNameType);}, 1000); // Hide the spinner after 1 seconds
    }
    hideSpinner(spinnerNameType : SpinnerType){
      this.spinner.hide(spinnerNameType); // Hide the spinner with a specific animation
    }
}

export enum SpinnerType {
  BallSquareMultiple = "ball-square-multiple",
  SquareJellyBox = "square-jelly-box"
}