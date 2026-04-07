import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, TranslateModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginData = {
    email: '',
    password: ''
  };

  error: string | null = null;

  constructor(private authService: AuthService, public router: Router, public translate: TranslateService) { }

  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (response: any) => {
        // نستخدم isSuccess كما هو موجود في الـ Service
        if (response && response.isSuccess) {
          console.log('Login Successful');
          // لا تقم بالحفظ يدوياً هنا، الـ Service قام بذلك بالفعل عبر setSession
          this.router.navigate(['/']);
        }
      },
      error: (err) => {
        const errorMessage = err.error?.message || 'Something went wrong';
        alert(errorMessage);
      }
    });
  }
  signInWithGoogle() {
    this.authService.loginWithExternalProvider('Google')
      .then(() => this.router.navigate(['/']))
      .catch((error) => alert(error.message));
  }

  signInWithFacebook() {
    this.authService.loginWithExternalProvider('Facebook')
      .then(() => this.router.navigate(['/']))
      .catch((error) => alert(error.message));
  }
}