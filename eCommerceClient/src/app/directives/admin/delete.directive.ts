import {
  Directive,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
  Renderer2
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent, DeleteState } from '../../dialogs/delete-dialog/delete-dialog.component';
import { HttpClientService } from '../../services/common/http-client.service';
import { AlertifyService, MessageType, Position } from '../../services/admin/alertify.service';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';
import { DialogService } from '../../services/common/dialog.service';

declare var $: any;

@Directive({
  selector: '[appDelete]',
  standalone: false
})
export class DeleteDirective {
  @Input() id!: string;
  @Input() controller : string;
  @Output() callback: EventEmitter<any> = new EventEmitter();

  constructor(
    private spinner : NgxSpinnerService,
    private element: ElementRef,
    private _renderer: Renderer2,
    private httpClientService : HttpClientService,
    private dialogService : DialogService,
    public dialog : MatDialog,
    public alertifyService : AlertifyService
  ) {
    const img = this._renderer.createElement('img');
    img.setAttribute('src', 'assets/delete.png');
    img.setAttribute('style', 'cursor: pointer;');
    img.width = 15;
    img.height = 15;
    this._renderer.appendChild(this.element.nativeElement, img);
  }

 @HostListener('click')
  async onClick() {
    this.dialogService.openDialog({
      componentType: DeleteDialogComponent,
      data: DeleteState.Yes,
      afterClosed: async () => {
        const td = this.element.nativeElement as HTMLTableCellElement;
      const parent = td?.parentElement;

      this.httpClientService.delete({
        controller: this.controller
      }, this.id).subscribe(data => {
        if (parent) {
          $(parent).fadeOut(1000, () => {
            this.callback.emit();
          });
        } else {
          this.callback.emit();
        }

        this.alertifyService.message("Successfully product deletion.", {
          DismissOthers: true,
          messageType: MessageType.Success,
          position: Position.TopRight
        });
      },
        (errorResponse: HttpErrorResponse) => {
          this.spinner.hide(SpinnerType.BallSquareMultiple)
          this.alertifyService.message("Product deletion failed", {
            DismissOthers: true,
            messageType: MessageType.Error,
            position: Position.TopRight
          });
        });
      }
    });
  }
}
