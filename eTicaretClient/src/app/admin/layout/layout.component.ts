import { Component, OnInit } from '@angular/core';
import { AlertifyService, MessageType, Position } from '../../services/admin/alertify.service';
declare var alertify: any; // Notify.js

@Component({
  selector: 'app-layout',
  standalone: false,
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit {
  constructor(private alertify: AlertifyService) {} // Inject AlertifyService

  ngOnInit(): void {
    this.alertify.message('Welcome to the Admin Layout!', {
      messageType: MessageType.Message, // Set message type
      position: Position.TopRight, // Set position
      delay: 5, // Set delay in seconds
      DismissOthers: true // Dismiss other notifications
    }); // Show a welcome message
    
    //this.alertify.dismiss(); // Dismiss any previous messages
  }  
}