import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../core/services/user/user.service';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RouterModule } from '@angular/router';

import { faUser } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-mange',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule, FormsModule, FontAwesomeModule],
  templateUrl: './user-mange.component.html',
  styleUrl: './user-mange.component.scss'
})
export class UserMangeComponent {
  user: any;
  defaultAvatar = 'https://www.gravatar.com/avatar?d=mp';
  userAvatarIcon = faUser;
  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userService.getUserById(id).subscribe({
        next: (res: any) => {
          this.user = res.data || res;
          console.log('User:', this.user);
        },
        error: (err) => console.error(err)
      });
    }
  }

  updateUser() {
    if (!this.user) return;

    this.userService.updateUser(this.user.userId, this.user).subscribe({
      next: () => {
        alert('User updated successfully');
      },
      error: (err) => console.error(err)
    });
  }

  deleteUser() {
    if (!this.user || !this.user.userId) return;

    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(this.user.userId).subscribe({
        next: (res) => {
          alert('User deleted successfully');
          this.router.navigate(['/admin/dashboard/manage-users']); // تأكد من صحة المسار هنا
        },
        error: (err) => {
          console.error('Delete error:', err);
          alert('Failed to delete user: ' + (err.error?.message || 'Server error'));
        }
      });
    }
  }

  toggleUserStatus(event: any) {
    this.user.isActive = event.target.checked;
    this.userService.updateUser(this.user.userId, { is_active: this.user.isActive }).subscribe({
      next: () => console.log('Status updated'),
      error: (err) => console.error(err)
    });
  }

  toggleEmailStatus(event: any) {
    this.user.emailVerified = event.target.checked;
    this.userService.updateUser(this.user.userId, { is_verified: this.user.emailVerified }).subscribe({
      next: () => console.log('Email status updated'),
      error: (err) => console.error(err)
    });
  }
}