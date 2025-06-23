import { Injectable } from '@angular/core';
declare var alertify: any; // Notify.js

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }
  // Notify.js methods
  // message(message: string, MessageType: MessageType, position: Position, delay: number, DismissOthers: boolean) {
  message(message: string, options: Partial<AlertifyOptions>) {  // Use Partial to allow partial configuration
    alertify.set('notifier', 'position', options.position);
    alertify.set('notifier', 'delay', options.delay);
    const msj = alertify[options.messageType](message);
    if (options.DismissOthers) {
      msj.dismissOthers();  // Dismiss other notifications
    }
  }

  dismiss() {
    alertify.dismissAll();  // Dismiss all notifications
  }
}

export class AlertifyOptions {
  messageType: MessageType = MessageType.Message;
  position: Position = Position.TopRight;
  delay: number = 3; // seconds
  DismissOthers: boolean = false; // Dismiss other notifications
}

export enum MessageType {
  Error = 'error',
  Message = 'message',
  Notify = 'notify',
  Success = 'success',
  Warning = 'warning'
}

export enum Position {
  TopLeft = 'top-left',
  TopRight = 'top-right',
  BottomLeft = 'bottom-left',
  BottomRight = 'bottom-right',
  TopCenter = 'top-center',
  BottomCenter = 'bottom-center'
}