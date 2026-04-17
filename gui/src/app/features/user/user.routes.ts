import { Routes } from '@angular/router';
import { ProfileComponent } from './pages/profile/profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserPropertyComponent } from './pages/user-property/user-property.component';


export const userRoutes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    children: [
   { path: '', redirectTo: 'profile', pathMatch: 'full' },
      { path: 'profile', component: ProfileComponent },
      { path: 'my-properties', component: UserPropertyComponent },
      // أضف الباقي هنا لاحقاً
    ]
  }
];
