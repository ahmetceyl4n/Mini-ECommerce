import { Component } from '@angular/core';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
import { AuthService } from './services/common/auth.service';
import { Router } from '@angular/router';
declare var $: any; // jQuery


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {

  constructor(
    private customToastrService: CustomToastrService,
    public authService: AuthService,
    private router: Router
  ) {
    this.customToastrService.message("Welcome to eTicaretClient!", "Welcome", {messageType: ToastrMessageType.Info, position: ToastrPosition.TopLeft});

    authService.identityCheck();
  }
  signOut() {
   this.authService.signOut();
   this.router.navigate(["login"]); // Kullanıcıyı login sayfasına yönlendir  
   this.customToastrService.message(
      "You have been signed out.",
      "Goodbye",
      { messageType: ToastrMessageType.Info, position: ToastrPosition.TopRight }
    );
  }
  
}
