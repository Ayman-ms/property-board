import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../../core/services/user/user.service';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  constructor(private router: Router, private userService: UserService, private authService: AuthService) { }
  user: any;
  editForm: any = {}; // variable to hold the edited user data
  isEditMode: boolean = false; // control the edit mode
  defaultAvatar: string = 'https://www.gravatar.com/avatar?d=mp';

  ngOnInit(): void {
    // retrieve user from localStorage
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      this.user = JSON.parse(storedUser);
      console.log("User loaded from localStorage:", this.user);
      this.editForm = { ...this.user };
    } else {
      // if not found locally, fetch from API using token
      const userId = this.authService.getCurrentUserId(); // or from JWT
      if (userId) {
        this.userService.getUserById(userId).subscribe({
          next: (res) => {
            this.user = res.data;
            this.editForm = { ...this.user };
          },
          error: (err) => console.error("Error loading profile", err)
        });
      }
    }
  }

    toggleEdit() {
    this.isEditMode = !this.isEditMode;
    if (!this.isEditMode) {
      this.editForm = { ...this.user };
    }
  }

  saveChanges() {
    // Call the update service from the backend here
    console.log("Saving data:", this.editForm);
    // After successful update on the server:
    this.user = { ...this.editForm };
    this.isEditMode = false;
    // alert('Profile Updated Successfully');
  }

  logout() {
    this.authService.logout();
  }
}
