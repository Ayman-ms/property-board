import { Component, Input } from '@angular/core'; // ضروري جداً
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faUser, faBars, faXmark } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-base-dashboard', // تأكد أن هذا الاسم يطابق الموجود في الـ HTML
  standalone: true,
  imports: [CommonModule, RouterModule, FontAwesomeModule],
  templateUrl: './base-daschbord.component.html',
  styleUrls: ['./base-daschbord.component.scss']
})
export class BaseDaschbordComponent {
  // يجب إضافة @Input قبل المتغيرات لكي يقبل تمرير البيانات من الخارج
  @Input() navItems: any[] = []; 
  @Input() accountTitle: string = 'Account';
  @Input() userAvatarIcon = faUser;

  // تعريف الأيقونات المستخدمة في قالب الـ Base نفسه
  faBars = faBars;
  faXmark = faXmark;
  faUser = faUser;

  sidebarOpen = false;

  toggleSidebar() { this.sidebarOpen = !this.sidebarOpen; }
  closeSidebar() { this.sidebarOpen = false; }
}