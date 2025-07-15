import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
email: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onResetPassword() {
    this.authService.resetPassword(this.email)
      .then(() => {
        alert('Check your email for reset link.');
        this.router.navigate(['/auth/login']);
      })
      .catch((error: { message: any; }) => {
        console.error(error.message);
        alert(error.message);
      });
  }
}