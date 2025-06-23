import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CustomToastrService {

  constructor(private toastr : ToastrService) { }
  message(message: string, title: string, ToastrOptions: Partial<ToastrOptions>) {
    this.toastr[ToastrOptions.messageType](message, title, {positionClass: ToastrOptions.position})
  }
}

export class ToastrOptions {
  messageType: ToastrMessageType = ToastrMessageType.Info;
  position: ToastrPosition = ToastrPosition.TopRight;
}

export enum ToastrMessageType {
  Error = 'error',
  Info = 'info',
  Success = 'success',
  Warning = 'warning'
}

export enum ToastrPosition {
  TopLeft = 'toast-top-left',
  TopRight = 'toast-top-right',
  BottomLeft = 'toast-bottom-left',
  BottomRight = 'toast-bottom-right',
  TopCenter = 'toast-top-center',
  BottomCenter = 'toast-bottom-center',
  TopFullWidth = 'toast-top-full-width',
  BottomFullWidth = 'toast-bottom-full-width'
}

