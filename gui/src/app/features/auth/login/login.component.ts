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

  constructor(private authService: AuthService, public router: Router, public translate: TranslateService) {}

onLogin() {
    console.log('Attempting login with:', this.loginData.email);

    this.authService.login(this.loginData).subscribe({
      next: (response: any) => {
        // فحص هيكلية الرد بناءً على ApiResponse في الباكيند
        if (response && response.success) {
          console.log('Login Successful');
          
          // حفظ التوكن (تأكد من أن الباكيند يعيد الحقل باسم token داخل data)
          localStorage.setItem('token', response.data.token);
          
          // التوجه للرئيسية
          this.router.navigate(['/']);
        } else {
          alert(response.message || 'Login failed');
        }
      },
      error: (err) => {
        console.error('Login Error:', err);
        // عرض رسالة الخطأ القادمة من ASP.NET (مثل Invalid password)
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