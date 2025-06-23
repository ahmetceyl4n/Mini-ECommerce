import { Component } from '@angular/core';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
declare var $: any; // jQuery


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'eTicaretClient';
  constructor(private customToastrService: CustomToastrService) {
    this.customToastrService.message("Welcome to eTicaretClient!", "Welcome", {messageType: ToastrMessageType.Info, position: ToastrPosition.TopLeft});
  }
}
/*
$(document).ready(function () {
alert("jQuery is working!") 
}) // Ensure jQuery is loaded and working
*/