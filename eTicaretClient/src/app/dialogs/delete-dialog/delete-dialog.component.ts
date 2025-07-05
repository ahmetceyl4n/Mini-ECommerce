import { Component, Inject, inject, model } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BaseDialogs } from '../base/base-dialogs';

@Component({
  selector: 'app-delete-dialog',
  standalone: false,
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.scss'
})
export class DeleteDialogComponent extends BaseDialogs<DeleteDialogComponent>{
  constructor(
    dialogRef : MatDialogRef<DeleteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data : DeleteState
  ){
    super(dialogRef)
  }
}

export enum DeleteState {
  Yes,
  No
}