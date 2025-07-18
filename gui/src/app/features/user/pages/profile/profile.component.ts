import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { UserService } from '../../../../core/services/user/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user: any = null;
  defaultAvatar: string = 'https://www.gravatar.com/avatar?d=mp';

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe((firebaseUser) => {
      if (firebaseUser) {
        console.log('Firebase user:', firebaseUser); // ✅
        this.userService.getUserById(firebaseUser.uid).subscribe((userData) => {
          console.log('User data from Firestore:', userData); // ✅
          this.user = userData;
        });
      }
    });
  }

  logout() {
    this.authService.logout().then(() => {
      this.router.navigate(['/auth/login']);
    });
  }

}
