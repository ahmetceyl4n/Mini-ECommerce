import { Component, OnDestroy } from '@angular/core';
import { UserService } from '../../../services/common/models/user.service';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from '../../../services/common/auth.service';
import {  ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

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
    private router: Router
  ) {
    super(spinner);
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
}
