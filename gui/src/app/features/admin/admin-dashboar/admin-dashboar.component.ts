import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {  faUser, faHome, faBell, faEnvelope, faBars, faXmark} from '@fortawesome/free-solid-svg-icons';
import { BaseDaschbordComponent } from '../../shared/base-daschbord/base-daschbord.component';

@Component({
  selector: 'app-admin-dashboar',
  standalone: true,
  imports: [CommonModule, RouterModule, FontAwesomeModule, BaseDaschbordComponent],
  templateUrl: './admin-dashboar.component.html',
  styleUrl: './admin-dashboar.component.scss'
})
export class AdminDashboarComponent {

  faUser = faUser;
  faHome = faHome;
  faBell = faBell;
  faEnvelope = faEnvelope;
  faBars = faBars;
  faXmark = faXmark;


  sidebarOpen = false;

  adminLinks = [
    { label: 'Profile', icon: faUser, route: 'admin-profile' },
    { label: 'Manage Properties', icon: faHome, route: 'manage-properties' },
    { label: 'Manage Users', icon: faBell, route: 'manage-users' },
    { label: 'Messages', icon: faEnvelope, route: 'messages' },
  ];
}

