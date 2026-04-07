import { Routes } from '@angular/router';
import { ProfileComponent } from './pages/profile/profile.component';


export const userRoutes: Routes = [
  { path: 'profile/:id', component: ProfileComponent },
];
