import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { User } from '../../../entities/user';
import { Create_User } from '../../../contracts/users/create_user';
import { firstValueFrom, Observable } from 'rxjs';
import { Token } from '../../../contracts/token/token';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { Position } from '../../admin/alertify.service';
import { TokenResponse } from '../../../contracts/token/tokenResponse';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private httpClientService: HttpClientService,
    private toastrService: CustomToastrService
  ) { }

  async create(user: User) : Promise<Create_User> {
    const observable : Observable<User | Create_User> = this.httpClientService.post<Create_User | User>({
      controller: 'users', 
    }, user)

    return await firstValueFrom(observable) as Create_User; 
    
    // dışarıdan gelen observable'ı Promise'e çeviriyoruz. Bu servis, dışarıdan bir User objesi alıp, onu API’ye POST eden ve API’den gelen sonucu Create_User tipinde döndüren bir metot sağlıyor 
  }
  async login(usernameOrEmail: string, password: string, callBackFunc?: () => void): Promise<any> {
    const observable: Observable<any | TokenResponse> = this.httpClientService.post<any | TokenResponse>({
      controller: 'users',
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



}
