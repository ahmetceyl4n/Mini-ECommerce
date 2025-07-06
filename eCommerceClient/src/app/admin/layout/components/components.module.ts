import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';
import {MatListModule} from '@angular/material/list';



@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    RouterModule, //Linklerin çalışması için import edilmeli
    MatListModule  // MatListModule, Angular Material'ın liste bileşenini kullanabilmemiz için gerekli
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,   // dışarıdan (örn. app.component'de) erişilebilmesi için
    FooterComponent
  ]
})
export class ComponentsModule { }
