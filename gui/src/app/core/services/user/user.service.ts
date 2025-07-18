import { Injectable } from '@angular/core';
import {  Firestore,  doc,  setDoc,  updateDoc,  docData } from '@angular/fire/firestore';
import { Observable } from 'rxjs';

export interface AppUser {
  uid: string;
  firstName: string;
  lastName: string;
  email: string;
  role: 'user' | 'admin';
  photoURL?: string;
  createdAt?: any;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private firestore: Firestore) {}

  getUserById(uid: string): Observable<AppUser> {
    const userRef = doc(this.firestore, `users/${uid}`);
    return docData(userRef, { idField: 'uid' }) as Observable<AppUser>;
  }

  async createUser(user: AppUser): Promise<void> {
    const userRef = doc(this.firestore, `users/${user.uid}`);
    await setDoc(userRef, user);
  }

  async updateUser(uid: string, data: Partial<AppUser>): Promise<void> {
    const userRef = doc(this.firestore, `users/${uid}`);
    await updateDoc(userRef, data);
  }
}
