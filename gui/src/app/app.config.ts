import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient, withInterceptors, HttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';

// Firebase
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { getAuth, provideAuth } from '@angular/fire/auth';
import { getFirestore, provideFirestore } from '@angular/fire/firestore';

// Translation & UI
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MessageService } from 'primeng/api';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';


export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

const firebaseConfig = {
  apiKey: "AIzaSyBgydXPQFFWcVeoMbtDVo3LI66nglEKpfE",
  authDomain: "property-board.firebaseapp.com",
  databaseURL: "https://property-board-default-rtdb.europe-west1.firebasedatabase.app",
  projectId: "property-board",
  storageBucket: "property-board.firebasestorage.app",
  messagingSenderId: "110040899024",
  appId: "1:110040899024:web:b48667d2e3058559336dd5"
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),

    // --- إعدادات فيربايس (يجب أن تبدأ بـ provideFirebaseApp) ---
    provideFirebaseApp(() => initializeApp(firebaseConfig)),
    provideAuth(() => getAuth()),
    provideFirestore(() => getFirestore()),

    // --- إعدادات الـ HTTP والـ Interceptor ---
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),

    // --- إعدادات الترجمة ---
    importProvidersFrom(
      TranslateModule.forRoot({
        defaultLanguage: 'en',
        loader: {
          provide: TranslateLoader,
          useFactory: createTranslateLoader,
          deps: [HttpClient],
        },
      })
    ),
    MessageService
  ]
};