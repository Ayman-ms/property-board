import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../../../shared/property-card/property-card.component';
import { UserService } from '../../../../core/services/user/user.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PropertyCardComponent],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {

 allUsers: any[] = [];
  defaultAvatar: string = 'https://www.gravatar.com/avatar?d=mp';

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUserProperties();
  }

  loadUserProperties() {
    this.userService.getUsers().subscribe({
      next: (res) => {
        this.allUsers = res;
        console.log("Loaded user properties:", this.allUsers);
      },
      error: (err) => console.error("Error loading user properties", err)
    });
  }
  
}