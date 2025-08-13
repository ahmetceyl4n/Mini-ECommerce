import { Component } from '@angular/core';
import { UserService } from '../../../services/common/models/user.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  constructor(
    private userService: UserService,
  ) {}

  async login(usernameOrEmail:string , password : string) {

    await this.userService.login(usernameOrEmail, password);
        

    }

}
