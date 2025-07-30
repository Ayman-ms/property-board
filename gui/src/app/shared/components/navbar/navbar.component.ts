import { Component } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../../core/services/theme/theme.service';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  isDarkMode = false;
  isLoggedIn = false;
  private authSub!: Subscription;

  constructor(
    public translate: TranslateService,
    private themeService: ThemeService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.authSub = this.authService.isLoggedIn$.subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
      console.log('User is logged in:', loggedIn);
    });
  }

  ngOnDestroy(): void {
    this.authSub.unsubscribe(); 
  }

  toggleTheme() {
    this.themeService.toggleTheme();
  }

}
