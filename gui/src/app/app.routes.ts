import { Routes } from '@angular/router';
import { ProfileComponent } from './features/user/pages/profile/profile.component';
import { authRoutes } from '../app/features/auth/auth.routes';
import { HomeComponent } from './features/pages/home/home.component';
import { propertyRoutes } from './features/properties/property.routes';
import { pagesRoutes } from './features/pages/pages.routes';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'auth', children: authRoutes },
  { path: 'properties', children: propertyRoutes },
  {path:'pages', children: pagesRoutes},
  { path: 'profile', component: ProfileComponent },
  {
    path: 'user/edit',
    loadComponent: () => import('./features/user/pages/edit-profile/edit-profile.component').then(m => m.EditProfileComponent)
  },
  //   {
  //   path: '**',
  //   loadComponent: () =>
  //     import('./shared/components/errors/not-found.component').then(m => m.NotFoundComponent)
  // }

];
