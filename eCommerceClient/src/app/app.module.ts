import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminModule } from './admin/admin.module';
import { UiModule } from './ui/ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr'; // Import ToastrModule for notifications
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

// Sosyal login için gerekli importlar
import { SocialLoginModule, SocialAuthServiceConfig, GoogleLoginProvider, FacebookLoginProvider, GoogleSigninButtonDirective } from '@abacritt/angularx-social-login';
import { LoginComponent } from './ui/components/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
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
    SocialLoginModule // Sosyal login için gerekli modül
    ,
    GoogleSigninButtonDirective
],
  providers: [
    {provide: "baseUrl", useValue: "https://localhost:7143/api", multi: true},
    {
      provide: "SocialAuthServiceConfig",
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider("594603581579-tap8bm8niss1t73t0rco4af8ljui0rum.apps.googleusercontent.com")  
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider("YOUR_FACEBOOK_APP_ID") 
          }
        ],
        onError: err => console.log(err)
      } as SocialAuthServiceConfig
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
