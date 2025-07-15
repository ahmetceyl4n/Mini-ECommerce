import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { User } from '../../../entities/user';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
}) 
export class RegisterComponent implements OnInit {
  constructor(private formBuilder: FormBuilder) {}
  
  frm : FormGroup;
  
  ngOnInit(): void {
    this.frm = this.formBuilder.group({
      nameSurname: ['' , [
        Validators.required, 
        Validators.minLength(3),
        Validators.maxLength(50)
        ]
      ],
      username: ['',
        [
          Validators.required, 
          Validators.minLength(3), 
          Validators.maxLength(20),
          Validators.pattern('^[a-zA-Z0-9_]+$') // Alphanumeric and underscores only
        ]
      ],
      email: ['' , [
        Validators.required,
        Validators.email,
        Validators.maxLength(100)
        ]
      ],
      password: ['' , [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$') // At least one lowercase, one uppercase, and one digit
        ]
      ],
      confirmPassword: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(20),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$') // Same pattern as password
        ]
      ]
    }, { validators: passwordMatchValidator('password', 'confirmPassword') });
  }

  get component() {
    return this.frm.controls;
  }

  onSubmit(data: User) {
    
  }
}

export function passwordMatchValidator(passwordKey: string, confirmPasswordKey: string): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const password = formGroup.get(passwordKey);
    const confirmPassword = formGroup.get(confirmPasswordKey);

    if (!password || !confirmPassword) return null;

    const mismatch = password.value !== confirmPassword.value;

    if (mismatch) {
      confirmPassword.setErrors({ ...confirmPassword.errors, passwordMismatch: true });
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