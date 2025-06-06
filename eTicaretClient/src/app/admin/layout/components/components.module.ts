import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    RouterModule //Linklerin çalışması için import edilmeli
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,   // dışarıdan (örn. app.component'de) erişilebilmesi için
    FooterComponent
  ]
})
export class ComponentsModule { }
