import { Injectable } from '@angular/core';
import { Firestore } from '@angular/fire/firestore';
import { doc, setDoc, updateDoc } from 'firebase/firestore';
import { docData } from '@angular/fire/firestore';
import { Observable } from 'rxjs';

export interface AppUser {
  uid: string;
  firstName: string;
  lastName: string;
  email: string;
  role: 'user' | 'admin';
  createdAt?: any;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private firestore: Firestore) {}

  /**
   * Get user data by UID
   */
  getUserById(uid: string): Observable<AppUser> {
    const userRef = doc(this.firestore, `users/${uid}`);
    return docData(userRef, { idField: 'uid' }) as Observable<AppUser>;
  }

  /**
   * Create a new user in Firestore
   */
  async createUser(user: AppUser): Promise<void> {
    const userRef = doc(this.firestore, `users/${user.uid}`);
    await setDoc(userRef, user);
  }

  /**
   * Update existing user data
   */
  async updateUser(uid: string, data: Partial<AppUser>): Promise<void> {
    const userRef = doc(this.firestore, `users/${uid}`);
    await updateDoc(userRef, data);
  }
}
