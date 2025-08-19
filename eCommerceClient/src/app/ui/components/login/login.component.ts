import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { UserService } from '../../../services/common/models/user.service';
import { AuthService } from '../../../services/common/auth.service';
import { HttpClientService } from '../../../services/common/http-client.service';

import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';

import {
  FacebookLoginProvider,
  SocialAuthService,
  SocialUser
} from '@abacritt/angularx-social-login';
import { UserAuthService } from '../../../services/common/models/user-auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],   
  standalone: false
})
export class LoginComponent extends BaseComponent implements OnDestroy {

  currentYear = new Date().getFullYear();

  private queryParamsSubscription?: Subscription;
  private authSubscription?: Subscription;

  constructor(
    private userAuthService: UserAuthService,
    spinner: NgxSpinnerService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private socialAuthService: SocialAuthService,
    private httpClientService: HttpClientService
  ) {
    super(spinner);

    // Sosyal giriş state'i dinle
    this.authSubscription = this.socialAuthService.authState.subscribe(async (user: SocialUser) => {
      if (!user) return;

      this.showSpinner(SpinnerType.SquareJellyBox);

      try {
        switch (user.provider) {
          case 'GOOGLE':
            await this.userAuthService.googleLogin(user, () => {
              this.authService.identityCheck();
            });
            break;
          case 'FACEBOOK':
            await this.userAuthService.facebookLogin(user, () => {
              this.authService.identityCheck();
            });
            break;
        }

        // Sosyal login sonrası geri dönüş adresi ya da ana sayfa
        const params = this.activatedRoute.snapshot.queryParams;
        const returnUrl = params['returnUrl'] || '/';
        await this.router.navigate([returnUrl]);
      } finally {
        this.hideSpinner(SpinnerType.SquareJellyBox);
      }
    });
  }

  async login(usernameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.SquareJellyBox);

    await this.userAuthService.login(usernameOrEmail, password, async () => {
      this.authService.identityCheck();

      // returnUrl yakala ve yönlendir
      this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe(async params => {
        const returnUrl = params['returnUrl'] || '/';
        await this.router.navigate([returnUrl]);
        this.hideSpinner(SpinnerType.SquareJellyBox);
      });
    });
  }

  facebookLogin() {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  ngOnDestroy() {
    this.queryParamsSubscription?.unsubscribe();
    this.authSubscription?.unsubscribe();
  }
}
