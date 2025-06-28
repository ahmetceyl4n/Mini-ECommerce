import {
  Directive,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
  Renderer2
} from '@angular/core';
import { ProductService } from '../../services/common/models/product.service';

declare var $: any;

@Directive({
  selector: '[appDelete]',
  standalone: false
})
export class DeleteDirective {

  @Input() id!: string;
  @Output() callback: EventEmitter<any> = new EventEmitter();

  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private productService: ProductService
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
    const td = this.element.nativeElement as HTMLTableCellElement;
    await this.productService.delete(this.id);

    const parent = td?.parentElement;
    if (parent) {
      $(parent).fadeOut(1000, () => {
        this.callback.emit();
      });
    } else {
      this.callback.emit();
    }
  }   // Silme ikonuna tıklandığında satırı siliyor ve otomatik olarak listeyi güncelliyor
}
