import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { ComponentsModule } from "./components/components.module";
import { Router, RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    LayoutComponent
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    RouterModule
],
  exports: [
    LayoutComponent   // dışarıdan (örn. app.component'de) erişilebilmesi için
  ]
})
export class LayoutModule { }
