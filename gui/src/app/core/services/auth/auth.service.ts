import { Injectable, inject } from '@angular/core';
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


  signInWithGoogle() {
    const provider = new GoogleAuthProvider();
    return signInWithPopup(this.auth, provider)
      .then(async (result) => {
        const user = result.user;

        // تحقق مما إذا كان المستخدم موجودًا مسبقًا في Firestore
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

  signInWithFacebook() {
    const provider = new FacebookAuthProvider();
    return signInWithPopup(this.auth, provider)
      .then(async (result) => {
        const user = result.user;
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

  login(email: string, password: string) {
    return signInWithEmailAndPassword(this.auth, email, password);
  }

  logout() {
    return this.auth.signOut();
  }


  getCurrentUser() {
    return authState(this.auth); // from firebase/auth
  }

  getauthState() {
    return this.auth;
  }
}