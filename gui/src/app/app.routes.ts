import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { HomeComponent } from './features/home/home.component';
import { ForgotPasswordComponent } from './features/auth/forgot-password/forgot-password.component';
import { ProfileComponent } from './features/user/pages/profile/profile.component';
import { EditProfileComponent } from './features/user/pages/edit-profile/edit-profile.component';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'profile', component: ProfileComponent },
  {
    path: 'user/edit',
    loadComponent: () => import('./features/user/pages/edit-profile/edit-profile.component').then(m => m.EditProfileComponent)
  },

];
