import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { User } from '../../../entities/user';
import { UserService } from '../../../services/common/models/user.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../../services/ui/custom-toastr.service';
import { Create_User } from '../../../contracts/users/create_user';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'] // Düzeltilmiş
})
export class RegisterComponent implements OnInit {
  frm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private toastrService: CustomToastrService
  ) {}

  ngOnInit(): void {
    this.frm = this.formBuilder.group(
      {
        nameSurname: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(50)
          ]
        ],
        username: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            Validators.pattern('^[a-zA-Z0-9_]+$') // Sadece harf, rakam ve alt çizgi
          ]
        ],
        email: [
          '',
          [
            Validators.required,
            Validators.email,
            Validators.maxLength(100)
          ]
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$'
            ) // En az 1 küçük, 1 büyük harf, 1 rakam
          ]
        ],
        PasswordConfirm: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$'
            ) // Şifre ile aynı kurallar
          ]
        ]
      },
      { validators: passwordMatchValidator('password', 'PasswordConfirm') }
    );
  }

  get component() {
    return this.frm.controls;
  }

  async onSubmit(user: User) {
    if (this.frm.invalid) {
      this.frm.markAllAsTouched();
      return;
    }

    const response : Create_User = await this.userService.create(user);

    if (response.succeeded) {
      this.toastrService.message(
        response.message,
        'Registration Successful',
        {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight
        }
      );
      this.frm.reset();
    } else {
      this.toastrService.message(
        response.message,
        'Registration Failed',
        {
          messageType: ToastrMessageType.Error,
          position: ToastrPosition.TopRight
        }
      );
    }
  }
}

export function passwordMatchValidator(
  passwordKey: string,
  confirmPasswordKey: string
): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const password = formGroup.get(passwordKey);
    const confirmPassword = formGroup.get(confirmPasswordKey);

    if (!password || !confirmPassword) return null;

    const mismatch = password.value !== confirmPassword.value;

    if (mismatch) {
      confirmPassword.setErrors({
        ...confirmPassword.errors,
        passwordMismatch: true
      });
    } else {
      if (confirmPassword.hasError('passwordMismatch')) {
        const errors = { ...confirmPassword.errors };
        delete errors['passwordMismatch'];
        if (Object.keys(errors).length === 0) {
          confirmPassword.setErrors(null);
        } else {
          confirmPassword.setErrors(errors);
        }
      }
    }

    return null;
  };
}
