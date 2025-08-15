import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminModule } from './admin/admin.module';
import { UiModule } from './ui/ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

// LoginComponent'i AppModule'den Kaldırıyoruz
// import { LoginComponent } from './ui/components/login/login.component';

// **LoginModule'u import ediyoruz** 
import { LoginModule } from './ui/components/login/login.module'; 

@NgModule({
  declarations: [
    AppComponent,
    // LoginComponent'i buradan kaldırdık
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AdminModule,
    UiModule,
    ToastrModule.forRoot(),
    NgxSpinnerModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem("accessToken");
        },
        allowedDomains: ["localhost:7143"],
        disallowedRoutes: []
      }
    }),
    // LoginModule'u burada import ediyoruz
    LoginModule  // LoginModule'u ekliyoruz
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
