import { Component, Input } from '@angular/core';
import { NgxFileDropEntry } from 'ngx-file-drop';
import { HttpClientService } from '../http-client.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AlertifyService, MessageType, Position } from '../../admin/alertify.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { MatDialog } from '@angular/material/dialog';
import { FileUploadDialogComponent } from '../../../dialogs/file-upload-dialog/file-upload-dialog.component';
import { DialogService } from '../dialog.service';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../../base/base.component';

@Component({
  selector: 'app-file-upload',
  standalone: false,
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {
  constructor(
    private httpClientService : HttpClientService,
    private alertifyService : AlertifyService,
    private customToastrService : CustomToastrService,
    private dialog : MatDialog,
    private dialogService : DialogService,
    private spinner : NgxSpinnerService
  ){}
  
  public files: NgxFileDropEntry[];

  @Input() options : Partial<FileUploadOptions>; 

  public selectedFiles(files: NgxFileDropEntry[]) { 
    this.files = files;
    const fileData : FormData = new FormData();
    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file : File) => {
        fileData.append(_file.name, _file, file.relativePath);
      })
    }

    this.dialogService.openDialog({
      componentType: FileUploadDialogComponent,
      data: FileUploadDialogState.Yes,
      afterClosed: () => {  
        this.spinner.show(SpinnerType.SquareJellyBox)
        this.httpClientService.post({
          controller : this.options.controller,
          action : this.options.action,
          queryString : this.options.queryString,
          headers : new HttpHeaders({"reponseType" : "blob"})
        }, fileData).subscribe(data => {
          const message: string = "Folder uploading successfully";
          if(this.options.isAdminPage){
            this.alertifyService.message(message, {
              DismissOthers : true, 
              messageType : MessageType.Success,
              position : Position.TopRight
            })
          } else{
            this.customToastrService.message(message, "Successful", {
              messageType : ToastrMessageType.Success,
              position : ToastrPosition.TopRight
            })
          }
          this.spinner.hide(SpinnerType.SquareJellyBox)
        },(errorResponse : HttpErrorResponse) => {
          const message: string = "Folder uploading not successfully";
          if(this.options.isAdminPage){
            this.alertifyService.message(message, {
              DismissOthers : true, 
              messageType : MessageType.Error,
              position : Position.TopRight
            })
          } else{
            this.customToastrService.message(message, "Fail", {
              messageType : ToastrMessageType.Error,
              position : ToastrPosition.TopRight
            })
          }
          this.spinner.hide(SpinnerType.SquareJellyBox)
        }); 
      }
    })
  }
}


export class FileUploadOptions {
  controller? : string;
  action? : string;
  queryString? : string;
  explanation? : string;
  accept? : string;
  isAdminPage? : boolean = false;
}

export enum FileUploadDialogState{
  Yes,
  No
}