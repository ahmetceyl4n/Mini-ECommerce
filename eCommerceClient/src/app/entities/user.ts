export class User {
  constructor(
    public nameSurname: string = '',
    public username: string = '',
    public email: string = '',
    public password: string = '',
    public confirmPassword: string = ''
  ) {}
}