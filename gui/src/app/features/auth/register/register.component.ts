import { CommonModule } from '@angular/common';
import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { Firestore, doc, setDoc, serverTimestamp } from '@angular/fire/firestore';
import { Storage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { Auth, createUserWithEmailAndPassword } from '@angular/fire/auth';;
import { TranslateModule, TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
     @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
 previewImage: string | null = null;
 selectedImage: File | null = null;

    user = {
    firstName: '',
    lastName: '',
    email: '',
    password: ''
  };
  error: string | null = null;
  
  constructor(private authService: AuthService, private firestore: Firestore,
    private storage: Storage, private auth: Auth,
    public translate: TranslateService
) {}

 triggerFileInput() {
   this.fileInput.nativeElement.click();
 }

 onFileSelected(event: any) {
   const file = event.target.files[0];
   if (file) {
     this.selectedImage = file;
     const reader = new FileReader();
     reader.onload = (e: any) => {
       this.previewImage = e.target.result;
     };
     reader.readAsDataURL(file);
   }
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

registerWithGoogle() {
  this.authService.loginOrRegisterWithGoogle().then(() => {
    console.log('Signed in with Google');
  }).catch(err => console.error(err));
}

registerWithFacebook() {
  this.authService.loginOrRegisterWithFacebook().then(() => {
    console.log('Signed in with Facebook');
  }).catch(err => console.error(err));
}

}
