import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, map, from } from 'rxjs';
import { ApiEndpoints } from '../../constants/api_endpoints';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Auth, signInWithPopup, GoogleAuthProvider, FacebookAuthProvider, signOut, authState, sendPasswordResetEmail } from '@angular/fire/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private loggedIn = new BehaviorSubject<boolean>(this.isAuthenticated());
  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient,
    private auth: Auth,
    private router: Router
  ) { }


  // login(credentials: any): Observable<any> {
  //   return this.http.post<any>(ApiEndpoints.auth.login, credentials).pipe(
  //     map(response => {
  //       if (response.success) {
  //         this.setSession(response.data);
  //       }
  //       return response;
  //     })
  //   );
  // }

  login(credentials: any) {
    return this.http.post<any>(`${ApiEndpoints.auth.login}`, credentials).pipe(
      map(response => {
        if (response && response.isSuccess && response.data.token) {
          // حفظ التوكن في المتصفح
          localStorage.setItem('token', response.data.token);
          this.setSession(response.data);
          // اختيارياً: حفظ بيانات المستخدم الأخرى
          localStorage.setItem('user', JSON.stringify(response.data.user));

          console.log('Token saved successfully!');
        }
        this.router.navigate(['/']);
        return response;
      })
    );
  }

  private externalLoginProvider(idToken: string, provider: string) {
    this.http.post<any>(`${ApiEndpoints.auth.login}/external`, { idToken, provider }).subscribe({
      next: (res) => {
        this.setSession(res.data);
        this.router.navigate(['/']);
      },
      error: (err) => console.error('External login failed', err)
    });
  }

  private setSession(authResult: any) {
    localStorage.setItem('token', authResult.token);
    const id = authResult.user?.userId || authResult.userId;
    if (id) {
      localStorage.setItem('userId', id.toString());
    }
    localStorage.setItem('role', authResult.user.role || 'user');
    localStorage.setItem('user', JSON.stringify(authResult.user));
    this.loggedIn.next(true);
  }

  logout() {
    localStorage.clear();
    this.loggedIn.next(false);
    signOut(this.auth); // تسجيل الخروج من Firebase أيضاً
    this.router.navigate(['/auth/login']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserRole(): string | null {
    return localStorage.getItem('role');
  }

  register(userData: FormData): Observable<any> {
    return this.http.post<any>(ApiEndpoints.auth.register, userData);
  }

  async loginWithExternalProvider(providerType: 'Google' | 'Facebook') {
    try {
      const provider = providerType === 'Google'
        ? new GoogleAuthProvider()
        : new FacebookAuthProvider();

      const result = await signInWithPopup(this.auth, provider);
      const idToken = await result.user.getIdToken();

      const payload = {
        idToken: idToken,
        provider: providerType,
        firstName: result.user.displayName?.split(' ')[0] || '',
        lastName: result.user.displayName?.split(' ')[1] || '',
        email: result.user.email
      };

      // نرسلها للباكيند (سواء كان تسجيل جديد أو دخول)
      this.http.post<any>(`${ApiEndpoints.auth.login}/external`, payload).subscribe({
        next: (res) => {
          this.setSession(res.data);
          this.router.navigate(['/']);
        },
        error: (err) => alert(`${providerType} Authentication failed: ` + err.message)
      });
    } catch (error: any) {
      console.error(`${providerType} Error:`, error);
      alert(error.message);
    }
  }

  resetPassword(email: string): Observable<any> {
    // Send password reset email using Firebase
    return this.http.post(`${ApiEndpoints.auth.baseUrl}/forgot-password`, { email });
  }

  confirmResetPassword(data: any): Observable<any> {
    return this.http.post(`${ApiEndpoints.auth.baseUrl}/reset-password`, data);
  }

  getauthState() {
    return this.auth;
  }
  
  getUserId(): string | null {
    let id = localStorage.getItem('userId');
    if (!id) {
      const userJson = localStorage.getItem('user');
      if (userJson) {
        const user = JSON.parse(userJson);
        id = user.userId || user.id;
      }
    }
    return id;
  }
}