import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, CommonModule, TranslateModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  email: string = '';
  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  isLoading: boolean = false;
  message: string = '';

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.token = this.route.snapshot.queryParamMap.get('token') || '';
    this.email = this.route.snapshot.queryParamMap.get('email') || '';

    if (!this.token || !this.email) {
      this.message = "This password reset link is invalid. Please request a new one.";
    }
  }

  onSubmit() {
    if (this.newPassword !== this.confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    this.isLoading = true;
    const resetData = {
      email: this.email,
      token: this.token,
      newPassword: this.newPassword
    };

    this.authService.confirmResetPassword(resetData).subscribe({
      next: (res) => {
        alert("Password changed successfully! You can now log in.");
        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        this.message = "Failed to reset password. The link may have expired.";
        this.isLoading = false;
      }
    });
  }
}