import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../services/ui/custom-toastr.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';
import { AuthService } from '../../services/common/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router, // Router, yönlendirme işlemleri için kullanılır
    private toastrService: CustomToastrService, // Kullanıcıya bildirimler göstermek için kullanılır
    private spinner: NgxSpinnerService, // Yükleme spinner'ı için kullanılır
    private authService: AuthService // Kimlik doğrulama durumunu kontrol etmek için kullanılır
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    this.spinner.show(SpinnerType.BallSquareMultiple); 

    // Kullanıcı giriş yapmamışsa
    if (!this.authService.isAuthenticated) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } }); 
      // Eğer token yoksa veya süresi dolmuşsa, kullanıcıyı login sayfasına yönlendiriyoruz.
      // queryParams ile geri dönülecek URL'yi de ekliyoruz, böylece kullanıcı giriş yaptıktan sonra bu sayfaya yönlendirilebilir.

      this.toastrService.message("You need to login to access this page", "Access Denied", {
        messageType: ToastrMessageType.Error,
        position: ToastrPosition.TopRight
      });

      this.spinner.hide(SpinnerType.BallSquareMultiple);
      return false; // Erişime izin vermiyoruz
    }

    this.spinner.hide(SpinnerType.BallSquareMultiple);
    return true; // Erişime izin veriyoruz
  }
}
