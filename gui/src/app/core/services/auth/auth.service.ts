import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Auth, signInWithEmailAndPassword, createUserWithEmailAndPassword, signOut, UserCredential, authState } from '@angular/fire/auth';
import { Firestore, doc, setDoc, getDoc, serverTimestamp } from '@angular/fire/firestore';
import { GoogleAuthProvider, FacebookAuthProvider, signInWithPopup } from 'firebase/auth';
import { sendPasswordResetEmail } from 'firebase/auth';

@Injectable({ providedIn: 'root' })
export class AuthService {

  constructor(private auth: Auth, private firestore: Firestore) { }

  async register(email: string, password: string, firstName: string, lastName: string) {
    const userCredential = await createUserWithEmailAndPassword(this.auth, email, password);
    const uid = userCredential.user.uid;

    const userData = {
      uid,
      email,
      firstName,
      lastName,
      role: 'user',
      createdAt: serverTimestamp()
    };

    await setDoc(doc(this.firestore, `users/${uid}`), userData);
  }

  private loggedIn = new BehaviorSubject<boolean>(this.isAuthenticated());
  isLoggedIn$ = this.loggedIn.asObservable();

  loginOrRegisterWithGoogle() {
    const provider = new GoogleAuthProvider();
    return signInWithPopup(this.auth, provider)
      .then(async (result) => {
        const user = result.user;

        const token = await user.getIdToken();
        localStorage.setItem('token', token);

        const userRef = doc(this.firestore, 'users', user.uid);
        const snapshot = await getDoc(userRef);
        if (!snapshot.exists()) {
          await setDoc(userRef, {
            email: user.email,
            firstName: user.displayName?.split(' ')[0] || '',
            lastName: user.displayName?.split(' ')[1] || '',
            role: 'member',
            createdAt: new Date().toISOString(),
            isApproved: false
          });
        }
      });
  }

  loginOrRegisterWithFacebook() {
    const provider = new FacebookAuthProvider();
    return signInWithPopup(this.auth, provider)
      .then(async (result) => {
        const user = result.user;

        const token = await user.getIdToken();
        localStorage.setItem('token', token);

        const userRef = doc(this.firestore, 'users', user.uid);
        const snapshot = await getDoc(userRef);
        if (!snapshot.exists()) {
          await setDoc(userRef, {
            email: user.email,
            firstName: user.displayName?.split(' ')[0] || '',
            lastName: user.displayName?.split(' ')[1] || '',
            role: 'member',
            createdAt: new Date().toISOString(),
            isApproved: false
          });
        }
      });
  }

  resetPassword(email: string) {
    return sendPasswordResetEmail(this.auth, email);
  }

  async login(email: string, password: string) {
    const userCredential = await signInWithEmailAndPassword(this.auth, email, password);
    const token = await userCredential.user.getIdToken();
    localStorage.setItem('token', token);
    this.loggedIn.next(true);

    return signInWithEmailAndPassword(this.auth, email, password)
      .then(async (userCredential) => {
        const token = await userCredential.user.getIdToken();
        localStorage.setItem('token', token);

        const userDoc = doc(this.firestore, `users/${userCredential.user.uid}`);
        const snapshot = await getDoc(userDoc);
        if (snapshot.exists()) {
          const role = snapshot.data()['role'];
          localStorage.setItem('role', role);
        }
        return userCredential;
      });
  }

  logout() {

    localStorage.removeItem('token');
    localStorage.removeItem('role');
    this.loggedIn.next(false);
    return this.auth.signOut();
  }


  getCurrentUser() {
    return authState(this.auth); // from firebase/auth
  }

  getauthState() {
    return this.auth;
  }

  getUserRole(): string | null {
    const role = localStorage.getItem('role');
    return role ? role : null;
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }
}