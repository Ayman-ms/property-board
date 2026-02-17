import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, TranslateModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
  email: string = '';

  constructor(private authService: AuthService, private router: Router, public translate: TranslateService) { }

  onResetPassword() {
    this.authService.resetPassword(this.email).subscribe({
      next: () => {
        alert('Check your email for reset link.');
        this.router.navigate(['/auth/login']);
      },
      error: (err: any) => { 
        console.error(err);
        const errorMessage = err.error?.message || 'Something went wrong';
        alert(errorMessage);
      }
    });
  }
}