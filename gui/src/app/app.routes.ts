import { Routes } from '@angular/router';
import { authRoutes } from '../app/features/auth/auth.routes';
import { HomeComponent } from './features/pages/home/home.component';
import { propertyRoutes } from './features/properties/property.routes';
import { pagesRoutes } from './features/pages/pages.routes';
import { ContactComponent } from './features/pages/contact/contact.component';
import { userRoutes } from './features/user/user.routes';
import { adminRoutes } from './features/admin/admin.routes';

import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { guestGuard } from './core/guards/guest.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },

  // public pages
  { path: 'home', component: HomeComponent },
  { path: 'properties', children: propertyRoutes },
  { path: 'pages', children: pagesRoutes },
  { path: 'contact', component: ContactComponent },

  // pages only for guests
  { 
    path: 'auth', 
    canActivate: [guestGuard],
    children: authRoutes 
  },

  //  admin pages
  { 
    path: 'admin', 
    canActivate: [adminGuard],
    canActivateChild: [adminGuard],
    children: adminRoutes 
  },

  // user pages
  { 
    path: 'user', 
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    children: userRoutes 
  },
  
  // صفحة عدم التصريح
 /* { 
    path: 'unauthorized',
    loadComponent: () => 
      import('./shared/components/unauthorized/unauthorized.component')
        .then(m => m.UnauthorizedComponent)
  }, */ 
  
  // أي رابط غير موجود → الرئيسية
  { path: '**', redirectTo: 'home' }
];