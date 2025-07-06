import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from './layout/layout.module';
import { ComponentsModule } from './layout/components/components.module';




@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    LayoutModule,     // Layout'u Admin'e import etmek için
    ComponentsModule  // Components'i Admin'e import etmek için
  ],
  exports: [
    LayoutModule,  // dışarıdan (örn. app.component'de) erişilebilmesi için
    CommonModule
  ]
})
export class AdminModule { }
