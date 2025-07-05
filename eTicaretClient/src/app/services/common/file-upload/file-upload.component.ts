import { Component, Input } from '@angular/core';
import { NgxFileDropEntry } from 'ngx-file-drop';
import { HttpClientService } from '../http-client.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AlertifyService, MessageType, Position } from '../../admin/alertify.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { MatDialog } from '@angular/material/dialog';

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
    private dialog : MatDialog
  ){}
  
  public files: NgxFileDropEntry[];

  @Input() options : Partial<FileUploadOptions>; 

  public selectedFiles(files: NgxFileDropEntry[]) { 
    
    openDialog(afterClosed : any): void {
        const dialogRef = this.dialog.open(DeleteDialogComponent, {
          data: DeleteState.Yes,
        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
          if (result == DeleteState.Yes) {
            afterClosed();
          }
        });
      }
    this.files = files;
    const fileData : FormData = new FormData();
    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file : File) => {
        fileData.append(_file.name, _file, file.relativePath);
      })
    }

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

export enum FileUploadDiialogState{
  Yes,
  No
}
