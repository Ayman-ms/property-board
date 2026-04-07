import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { UserService } from '../../../../core/services/user/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms'; // ضروري لـ ngModel

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule], // أضف FormsModule
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user: any;
  editForm: any = {}; // كائن مؤقت للتعديل
  isEditMode: boolean = false; // التحكم في وضع الصفحة
  defaultAvatar: string = 'https://www.gravatar.com/avatar?d=mp';

  constructor(
    private authService: AuthService,
    private userService: UserService,
    public router: Router,
    public translate: TranslateService,
    public route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.userService.getUserById(userId).subscribe({
        next: (res) => {
          this.user = res.data;
          // نأخذ نسخة من البيانات لوضع التعديل حتى لا نعدل الأصل فوراً
          this.editForm = { ...this.user }; 
        },
        error: (err) => console.error("Error loading profile", err)
      });
    }
  }

  toggleEdit() {
    this.isEditMode = !this.isEditMode;
    if (!this.isEditMode) {
      this.editForm = { ...this.user }; // استعادة البيانات الأصلية عند الإلغاء
    }
  }

  saveChanges() {
    // هنا تستدعي خدمة التحديث من الباكيند
    console.log("Saving data:", this.editForm);
    // بعد نجاح التحديث في السيرفر:
    this.user = { ...this.editForm };
    this.isEditMode = false;
    // alert('Profile Updated Successfully');
  }

  logout() {
    this.authService.logout();
  }
}