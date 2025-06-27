import { Component, OnInit } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClientService } from '../../../services/common/http-client.service';
import { Create_Product } from '../../../contracts/create_product';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService, private httpClientService: HttpClientService) { 
    super(spinner); // Call the constructor of the base class
  }
  ngOnInit(): void {
    // Initialize the component
    this.showSpinner(SpinnerType.SquareJellyBox); // Show the loading spinner
    this.httpClientService.get<Create_Product>({
      controller: "products"
    }).subscribe(data => console.log(data))

    /*  //Post kısmı. Sürekli oluşturmasın diye yorumlandı
    this.httpClientService.post({
      controller: "products"
    }, {
      name: "Pencil",
      stock: 100,
      price: 15
    }).subscribe();

    this.httpClientService.post({
      controller: "products"
    }, {
      name: "Paper",
      stock: 50,
      price: 5
    }).subscribe();

    this.httpClientService.post({
      controller: "products"
    }, {
      name: "Sharper",
      stock: 200,
      price: 10
    }).subscribe();

    this.httpClientService.post({
      controller: "products"
    }, {
      name: "Eraser",
      stock: 150,
      price: 10
    }).subscribe();

    this.httpClientService.post({
      controller: "products"
    }, {
      name: "Pen box",
      stock: 60,
      price: 20
    }).subscribe();
    */

    /*  //Updating
    this.httpClientService.put({
      controller: "products"
    }, {
      id: "0197b19a-9167-78e8-a344-964d9e38cae5",
      name: "eraser box",
      stock: 1000,
      price: 30
    }).subscribe();
    */
    /*  
    this.httpClientService.delete({
      controller: "products"
    }, "0197b19a-9167-78e8-a344-964d9e38cae5"
    ).subscribe(data => console.log(data));
    */
  }
}