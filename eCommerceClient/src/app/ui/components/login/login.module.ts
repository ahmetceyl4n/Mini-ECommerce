import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { GoogleSigninButtonDirective } from "@abacritt/angularx-social-login";


@NgModule({
  declarations: [
    LoginComponent  // LoginComponent burada tanımlı
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
        { path: "", component: LoginComponent }
    ]),
    ReactiveFormsModule // Eğer formlar kullanıyorsanız
    ,
    GoogleSigninButtonDirective
]
})
export class LoginModule { }
