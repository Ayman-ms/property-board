import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AppUser, UserService } from '../../../../core/services/user/user.service';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss'
})
export class EditProfileComponent implements OnInit {
  profileForm!: FormGroup;
  userId: string = '';
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    public translate: TranslateService
  ) {}

  ngOnInit() {
    this.authService.getCurrentUser().subscribe(firebaseUser => {
      if (firebaseUser) {
        this.userId = firebaseUser.uid;
        this.userService.getUserById(this.userId).subscribe((user: AppUser) => {
          this.profileForm = this.fb.group({
            firstName: [user.firstName],
            lastName: [user.lastName],
            email: [{ value: user.email, disabled: true }],
            photoURL: [user.photoURL],
            role: [user.role],
          });
          this.isLoading = false;
        });
      }
    });
  }

  async saveProfile() {
    if (this.profileForm.valid) {
      await this.userService.updateUser(this.userId, this.profileForm.getRawValue());
      alert('تم تحديث الملف الشخصي بنجاح!');
      this.router.navigate(['/user/profile']);
    }
  }
}
