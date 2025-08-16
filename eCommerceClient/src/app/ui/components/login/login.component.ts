import { Component, OnDestroy } from '@angular/core';
import { UserService } from '../../../services/common/models/user.service';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from '../../../services/common/auth.service';
import {  ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FacebookLoginProvider, SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { HttpClientService } from '../../../services/common/http-client.service';
import { TokenResponse } from '../../../contracts/token/tokenResponse';
@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends BaseComponent implements OnDestroy {

  currentYear = new Date().getFullYear();
  private queryParamsSubscription: Subscription; // queryParams subscribe için subscription

  constructor(
    private userService: UserService,
    spinner: NgxSpinnerService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private socialAuthService: SocialAuthService,
    private httpClientService: HttpClientService
  ) {
    super(spinner);
    socialAuthService.authState.subscribe(async (user: SocialUser)  => {
      
      this.showSpinner(SpinnerType.SquareJellyBox);

      switch (user.provider) {
        case "GOOGLE":
          await userService.googleLogin(user, () => {
            authService.identityCheck();
            this.hideSpinner(SpinnerType.SquareJellyBox)})
          break;
        case "FACEBOOK":
          await userService.facebookLogin(user, () => {
            authService.identityCheck();
            this.hideSpinner(SpinnerType.SquareJellyBox)});
          break;  

      }
    });
  }

  async login(usernameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.SquareJellyBox);
    await this.userService.login(usernameOrEmail, password, () => { 
      this.authService.identityCheck();

      // queryParams subscription
      this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe(params => {
        const returnUrl = params['returnUrl'] || '/'; // Varsayılan olarak anasayfaya yönlendir
        this.router.navigate([returnUrl]).then(() => {
          this.hideSpinner(SpinnerType.SquareJellyBox)
        });
      });
    });
  }

  ngOnDestroy() {
    // component destroy olduğunda subscription'ı temizle
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }
  }


  facebookLogin() {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }
}
