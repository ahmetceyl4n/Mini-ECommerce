import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { User } from '../../../entities/user';
import { Create_User } from '../../../contracts/users/create_user';
import { firstValueFrom, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClientService: HttpClientService) { }

  async create(user: User) : Promise<Create_User> {
    const observable : Observable<User | Create_User> = this.httpClientService.post<Create_User | User>({
      controller: 'users', 
    }, user)

    return await firstValueFrom(observable) as Create_User; 
    
    // dışarıdan gelen observable'ı Promise'e çeviriyoruz. Bu servis, dışarıdan bir User objesi alıp, onu API’ye POST eden ve API’den gelen sonucu Create_User tipinde döndüren bir metot sağlıyor 
  }
  async login(usernameOrEmail: string, password: string): Promise<void> {
    const observable: Observable<any> = this.httpClientService.post({
      controller: 'users',
      action: 'login'
    }, { usernameOrEmail, password }); // API'ye kullanıcı adı veya e-posta ve şifre ile giriş yapma isteği gönderiyoruz

    return await firstValueFrom(observable);
  }



}
