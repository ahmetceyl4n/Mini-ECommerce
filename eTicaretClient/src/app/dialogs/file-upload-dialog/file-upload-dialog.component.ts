import { Component, Inject } from '@angular/core';
import { BaseDialogs } from '../base/base-dialogs';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-file-upload-dialog',
  standalone: false,
  templateUrl: './file-upload-dialog.component.html',
  styleUrl: './file-upload-dialog.component.scss'
})
export class FileUploadDialogComponent extends BaseDialogs<FileUploadDialogComponent> {
  constructor( dialogRef : MatDialogRef<FileUploadDialogComponent>,
  @Inject(MAT_DIALOG_DATA) public data : UploadState
  ){
     super(dialogRef)
  }
}

export enum UploadState {
  Yes,
  No
}