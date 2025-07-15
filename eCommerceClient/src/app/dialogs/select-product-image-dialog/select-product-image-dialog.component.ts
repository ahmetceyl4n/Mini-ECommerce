import { Component, Inject, OnInit, Output } from '@angular/core';
import { BaseDialogs } from '../base/base-dialogs';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FileUploadOptions } from '../../services/common/file-upload/file-upload.component';
import { List_Product_Image } from '../../contracts/list_product_image';
import { ProductService } from '../../services/common/models/product.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';
import { DialogService } from '../../services/common/dialog.service';
import { DeleteDialogComponent, DeleteState } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-select-product-image-dialog',
  standalone: false,
  templateUrl: './select-product-image-dialog.component.html',
  styleUrl: './select-product-image-dialog.component.scss'
})
export class SelectProductImageDialogComponent extends BaseDialogs<SelectProductImageDialogComponent> implements OnInit {
  data: SelectProductImageState | string;

  options!: Partial<FileUploadOptions>;

  constructor(
    dialogRef: MatDialogRef<SelectProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public injectedData: SelectProductImageState | string,
    private productService : ProductService,
    private spinner : NgxSpinnerService,
    private dialogService : DialogService
    ) {
    super(dialogRef);
    this.data = injectedData;
  }

  images : List_Product_Image[];

  async ngOnInit() {
    this.options = {
      accept: ".png, .jpg, .jpeg, .gif",
      action: "upload",
      controller: "products",
      explanation: "Drag and drop pictures...",
      isAdminPage: true,
      queryString: `id=${this.data}`,
    };
    
    this.spinner.show(SpinnerType.BallSquareMultiple);
    
    this.images = await this.productService.readImages(this.data as string, () => this.spinner.hide(SpinnerType.BallSquareMultiple));
    }

    async deleteImage(imageId: string) {

      this.dialogService.openDialog({
        componentType: DeleteDialogComponent,
        data: DeleteState.Yes,
        afterClosed: async () => {
          this.spinner.show(SpinnerType.BallSquareMultiple)
          await this.productService.deleteImage(this.data as string, imageId, () => {
            this.spinner.hide(SpinnerType.BallSquareMultiple);                  
          });
        }
      })
    }
  }

export enum SelectProductImageState {
  Close
}
