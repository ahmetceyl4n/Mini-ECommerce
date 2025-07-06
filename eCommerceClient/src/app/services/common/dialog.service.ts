import { ComponentType } from '@angular/cdk/overlay';
import { Component, Injectable } from '@angular/core';
import { DialogPosition, MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog : MatDialog) { }

  openDialog(dialogParameters : Partial<DialogParameters>): void {
        const dialogRef = this.dialog.open(dialogParameters.componentType, {
          width: dialogParameters.options?.width,
          height: dialogParameters.options?.height,
          position: dialogParameters.options?.position,
          data: dialogParameters.data,
        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
          if (result == dialogParameters.data) {
            dialogParameters.afterClosed();
          }
        });
      }
}

export class DialogParameters {
  componentType: ComponentType<any>;
  data?: any;
  afterClosed?: () => void;
  options?: Partial<DialogOptions>;
}

export class DialogOptions {
  width?: string = "250px";
  height?: string;
  disableClose?: boolean;
  hasBackdrop?: boolean;
  backdropClass?: string;
  panelClass?: string;
  position?: DialogPosition;
  autoFocus?: boolean;
  restoreFocus?: boolean;
  closeOnNavigation?: boolean;
}