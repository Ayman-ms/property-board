import { Component } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../../core/services/theme/theme.service';
import { AuthService } from '../../../core/services/auth/auth.service';
import { RouterLink } from '@angular/router';
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
currentUserId: string | null = null;

  constructor(
    public translate: TranslateService,
    private themeService: ThemeService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
   this.authSub= this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
      if (status) {
      // حاول جلب الـ ID
      this.currentUserId = this.authService.getUserId();
      
      // إذا كان لا يزال null، اطبع تحذيراً لنعرف السبب
      if (!this.currentUserId) {
        console.warn('User is logged in but ID is missing in localStorage!');
      }
    } else {
      this.currentUserId = null;
    }
    });
    const savedTheme = localStorage.getItem('theme');
    this.isDarkMode = savedTheme === 'dark';
    this.applyTheme();
  }

  ngOnDestroy(): void {
    if (this.authSub) {
      this.authSub.unsubscribe();
    }
  }

  toggleTheme() {
    this.themeService.toggleTheme();
    this.isDarkMode = !this.isDarkMode;

    localStorage.setItem('theme', this.isDarkMode ? 'dark' : 'light');
    this.applyTheme();
  }

  private applyTheme() {
    if (this.isDarkMode) {
      document.body.classList.add('dark-mode');
    } else {
      document.body.classList.remove('dark-mode');
    }
  }

}
