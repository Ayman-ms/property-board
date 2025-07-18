import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Firestore, doc, setDoc, serverTimestamp } from '@angular/fire/firestore';
import { Storage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { Auth, createUserWithEmailAndPassword } from '@angular/fire/auth';;

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
    selectedImage: File | null = null;

    user = {
    firstName: '',
    lastName: '',
    email: '',
    password: ''
  };
  error: string | null = null;
  
  constructor(private authService: AuthService, private firestore: Firestore,
    private storage: Storage, private auth: Auth
) {}

onFileSelected(event: any) {
    this.selectedImage = event.target.files[0];
  }


async onSubmit(form: any) {
    const { email, password, firstName, lastName } = form.value;

    try {
      const userCredential = await createUserWithEmailAndPassword(this.auth, email, password);
      const uid = userCredential.user.uid;

      let photoURL = `https://ui-avatars.com/api/?name=${firstName}+${lastName}&background=random`;


      if (this.selectedImage) {
        const filePath = `profileImages/${uid}`;
        const storageRef = ref(this.storage, filePath);
        await uploadBytes(storageRef, this.selectedImage);
        photoURL = await getDownloadURL(storageRef);
      }

      await setDoc(doc(this.firestore, 'users', uid), {
        uid,
        email,
        firstName,
        lastName,
        photoURL,
        role: 'user',
        createdAt: serverTimestamp()
      });

      console.log('User registered successfully');

    } catch (error) {
      console.error('Registration error:', error);
    }
  }

signInWithGoogle() {
  this.authService.signInWithGoogle().then(() => {
    console.log('Signed in with Google');
  }).catch(err => console.error(err));
}

signInWithFacebook() {
  this.authService.signInWithFacebook().then(() => {
    console.log('Signed in with Facebook');
  }).catch(err => console.error(err));
}

}
