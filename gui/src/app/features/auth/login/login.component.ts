import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';

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

  constructor(private authService: AuthService,
    public router: Router,
    public translate: TranslateService,
    private route: ActivatedRoute
  ) { }

  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (response: any) => {
        if (response && response.isSuccess) {
          console.log('Login Successful');

          //first check returnUrl (if user was trying to access a protected page)
          const returnUrl = this.route.snapshot.queryParams['returnUrl'];

          if (returnUrl) {
            this.router.navigateByUrl(returnUrl);
          } else if (this.authService.isAdmin()) {
            // If admin, redirect to admin dashboard
            this.router.navigate(['/admin/dashboard']);
          } else {
            // Regular user → home page
            this.router.navigate(['/']);
          }
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