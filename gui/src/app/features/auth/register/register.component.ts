import { CommonModule } from '@angular/common';
import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  previewImage: string | null = null;
  selectedImage: File | null = null;

  user = {
    firstName: '',
    lastName: '',
    email: '',
    password: ''
  };
  error: string | null = null;

  constructor(private authService: AuthService,
    private router: Router,
    public translate: TranslateService
  ) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedImage = file;
      const reader = new FileReader();
      reader.onload = (e: any) => this.previewImage = e.target.result;
      reader.readAsDataURL(file);
    }
  }

onSubmit(form: any) {
    if (form.invalid) return;

    // تحويل البيانات إلى FormData لأننا نرسل ملفاً (IFormFile)
    const formData = new FormData();
    formData.append('firstName', this.user.firstName);
    formData.append('lastName', this.user.lastName);
    formData.append('email', this.user.email);
    formData.append('password', this.user.password);
    
    if (this.selectedImage) {
      formData.append('profileImage', this.selectedImage);
    }

    this.authService.register(formData).subscribe({
      next: (res) => {
        alert('Registered successfully!');
        this.router.navigate(['/auth/login']);
      },
      error: (err) => alert(err.error?.message || 'Error occurred')
    });
  }

  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }
}


  // registerWithGoogle() {
  //   this.authService.loginOrRegisterWithGoogle().then(() => {
  //     console.log('Signed in with Google');
  //   }).catch(err => console.error(err));
  // }

  // registerWithFacebook() {
  //   this.authService.loginOrRegisterWithFacebook().then(() => {
  //     console.log('Signed in with Facebook');
  //   }).catch(err => console.error(err));
  // }