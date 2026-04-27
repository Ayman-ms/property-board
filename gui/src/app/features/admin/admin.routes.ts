import { Routes } from '@angular/router';
import { AdminDashboarComponent } from './admin-dashboar/admin-dashboar.component';
import { AdminPropertiesComponent } from './pages/admin-properties/admin-properties.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { UsersComponent } from './pages/users/users.component';
import { UserMangeComponent } from './pages/user-mange/user-mange.component';
export const adminRoutes: Routes = [
  {
    path: 'dashboard',
    component: AdminDashboarComponent,
    children: [
   { path: '', redirectTo: 'admin-profile', pathMatch: 'full' },
        { path: 'admin-profile', component: ProfileComponent },
        { path: 'manage-properties', component: AdminPropertiesComponent },
        { path: 'manage-users', component: UsersComponent },
        { path: 'user/:id', loadComponent: () =>
    import('./pages/user-mange/user-mange.component')
      .then(m => m.UserMangeComponent)
 },

      // أضف الباقي هنا لاحقاً
    ]
  }
];
