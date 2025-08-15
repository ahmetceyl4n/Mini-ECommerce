import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Kullanıcının kimlik doğrulamasının yapılıp yapılmadığını tutar
  // (private yaparak dışarıdan değiştirilmesini engelliyoruz)
  private _isAuthenticated: boolean = false;

  constructor(
    private jwtHelper: JwtHelperService, // JwtHelperService, JWT token'ları çözümlemek ve doğrulamak için kullanılır
  ) { }

  identityCheck() { 
    const token = localStorage.getItem("accessToken");

    // const decodeToken = this.jwtHelper.decodeToken(token); // Token'ı çözümleyerek içeriğini alıyoruz
    // const expirationDate = this.jwtHelper.getTokenExpirationDate(token); // Token'ın son kullanma tarihini alıyoruz
    // const isExpired = this.jwtHelper.isTokenExpired(token); // Token'ın süresinin dolup dolmadığını kontrol ediyoruz

    let isExpired: boolean;

    try {
      isExpired = this.jwtHelper.isTokenExpired(token); 
    } catch (error) {
      isExpired = true; // Token geçersizse süresi dolmuş olarak kabul ediyoruz
    }

    // Token varsa ve süresi dolmamışsa, kullanıcı kimlik doğrulaması yapılmış olarak kabul edilir
    this._isAuthenticated = token != null && !isExpired;
  }

  // Kullanıcının kimlik doğrulamasının yapılıp yapılmadığını döndürür
  get isAuthenticated(): boolean {
    return this._isAuthenticated;
  }

  signOut() {
    localStorage.removeItem("accessToken"); // Token'ı localStorage'dan kaldırıyoruz
    this._isAuthenticated = false; // Kimlik doğrulamasını false olarak ayarlıyoruz
  }
}
