import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { Create_Product } from '../../../contracts/create_product';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }
  create(product: Create_Product, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void) {
    this.httpClientService.post({
      controller: "products"
    }, product)
      .subscribe(result => {
        successCallBack();
      },(errorResponse: HttpErrorResponse) => {
        const validationErrors = errorResponse.error;

        let message = "";

        // validationErrors bir object, bu nedenle Object.entries() kullanmalıyız
        for (let [field, errors] of Object.entries(validationErrors)) {
          (errors as string[]).forEach(err => {
            message += `${err}<br>`;
          });
        }

        if (errorCallBack) errorCallBack(message);
      });
  }
}
