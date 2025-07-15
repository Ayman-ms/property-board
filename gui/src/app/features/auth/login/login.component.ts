import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginData = {
    email: '',
    password: ''
  };

  error: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.authService.login(this.loginData.email, this.loginData.password)
      .then(() => {
        console.log('Logged in successfully');
        this.router.navigate(['/']); // وجه المستخدم للصفحة الرئيسية أو لوحة التحكم
      })
      .catch((error) => {
        console.error(error.message);
        alert(error.message); // يمكن تحسينها لاحقًا باستخدام toast
      });
  }

  signInWithGoogle() {
    this.authService.signInWithGoogle()
      .then(() => this.router.navigate(['/']))
      .catch((error) => alert(error.message));
  }

  signInWithFacebook() {
    this.authService.signInWithFacebook()
      .then(() => this.router.navigate(['/']))
      .catch((error) => alert(error.message));
  }
}