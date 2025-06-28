import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../services/common/models/product.service';
import { Create_Product } from '../../../../contracts/create_product';
import { BaseComponent, SpinnerType } from '../../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService, MessageType, Position } from '../../../../services/admin/alertify.service';

@Component({
  selector: 'app-create',
  standalone: false,
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export class CreateComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService, private productService: ProductService, private alertify: AlertifyService){
    super(spinner)
  }

  ngOnInit(): void {
    
  }

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {
    this.showSpinner(SpinnerType.BallSquareMultiple);
    const create_product: Create_Product = new Create_Product();
    create_product.name = name.value;
    create_product.stock = parseInt(stock.value);
    create_product.price = parseFloat(price.value);

    if(!name.value){
      this.alertify.message("Product name cannot be empty.", {
        DismissOthers: true,
        messageType: MessageType.Error,
        position: Position.TopRight
      });      
      return; 
    }

    this.productService.create(create_product, () => {
      this.hideSpinner(SpinnerType.BallSquareMultiple);
      this.alertify.message("Successfully product creation.", {
        DismissOthers: true,
        messageType: MessageType.Success,
        position: Position.TopRight
      });
      
    }, errorMessage => {
      this.hideSpinner(SpinnerType.BallSquareMultiple);
      this.alertify.message(errorMessage,
        {
          DismissOthers: true,
          messageType: MessageType.Error,
          position: Position.TopRight
        });
    });
  }
}