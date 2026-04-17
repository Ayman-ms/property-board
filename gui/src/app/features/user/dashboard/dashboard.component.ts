import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faUser, faHome, faBell, faEnvelope, faBars, faXmark
} from '@fortawesome/free-solid-svg-icons';
import { filter } from 'rxjs';
import { BaseDaschbordComponent } from '../../shared/base-daschbord/base-daschbord.component';
 
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet, FontAwesomeModule, BaseDaschbordComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

  // ─── Icons ───────────────────────────────────────────
  faUser     = faUser;
  faHome     = faHome;
  faBell     = faBell;
  faEnvelope = faEnvelope;
  faBars     = faBars;
  faXmark    = faXmark;
 
  // ─── Sidebar State ───────────────────────────────────
  sidebarOpen = false;
 
  userLinks = [
    { label: 'Profile',        icon: faUser,     route: 'profile'       },
    { label: 'My Properties',  icon: faHome,     route: 'my-properties' },
    { label: 'Notifications',  icon: faBell,     route: 'notifications' },
    { label: 'Messages',       icon: faEnvelope, route: 'messages'      },
  ];
 
  constructor(private router: Router) {
    // يغلق الـ sidebar تلقائياً عند التنقل على الموبايل
    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd)
    ).subscribe(() => {
      this.sidebarOpen = false;
    });
  }
 
  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }
 
  closeSidebar() {
    this.sidebarOpen = false;
  }
 
  isActive(route: string): boolean {
    return this.router.url.includes(route);
  }
}
