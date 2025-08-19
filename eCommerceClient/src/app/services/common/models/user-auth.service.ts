import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { firstValueFrom, Observable } from 'rxjs';
import { TokenResponse } from '../../../contracts/token/tokenResponse';
import { SocialUser } from '@abacritt/angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {
  constructor(
      private httpClientService: HttpClientService,
      private toastrService: CustomToastrService
    ) { }
  
  async login(usernameOrEmail: string, password: string, callBackFunc?: () => void): Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse>({
      controller: 'auth',
      action: 'login'
    }, { usernameOrEmail, password }); // API'ye kullanıcı adı veya e-posta ve şifre ile giriş yapma isteği gönderiyoruz

    const tokenResponse : TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if(tokenResponse) 
    {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken); // Token'ı localStorage'a kaydediyoruz
      //localStorage.setItem("expiration", token.expiration.toString()); // Token'ın süresini localStorage'a kaydediyoruz

      this.toastrService.message("Successfully logged in", "Login Successful", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }

    callBackFunc(); 
  }


  async googleLogin(user : SocialUser,callBackFunc?: () => void) : Promise<any> { 

    const observable : Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      action: "google-login",
      controller: "auth"
      }, user)

    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;  
    
    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken); // Token'ı localStorage'a kaydediyoruz
      
      this.toastrService.message("Successfully logged in with Google", "Login Successful", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });      
    }
    callBackFunc();
  }

  async facebookLogin(user: SocialUser, callBackFunc?: () => void): Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      action: "facebook-login",
      controller: "auth"
    }, user);

    const tokenResponse : TokenResponse = await firstValueFrom(observable) as TokenResponse;

    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken); // Token'ı localStorage'a kaydediyoruz
      
      this.toastrService.message("Successfully logged in with Facebook", "Login Successful", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }
    callBackFunc();
  }

}
