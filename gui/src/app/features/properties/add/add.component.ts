import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { Firestore, collection, addDoc, serverTimestamp } from '@angular/fire/firestore';
import { getStorage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { v4 as uuidv4 } from 'uuid';



@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule],
  providers: [],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {

  previewImages: string[] = [];
  selectedFiles: File[] = [];

  zipCode = '';
  country = '';
  state = '';
  city = '';

  constructor(private http: HttpClient, private firestore: Firestore) {}

  lookupZip() {
    if (!this.zipCode || this.zipCode.length < 4) return;

    const url = `https://api.zippopotam.us/${this.country}/${this.zipCode}`;
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const place = res.places[0];
        this.city = place['place name'];
        this.state = place['state'];
      },
      error: () => {
        this.city = '';
        this.state = '';
        alert('ZIP code not found');
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const newFiles = Array.from(input.files);
      for (const file of newFiles) {
        this.selectedFiles.push(file);
        const reader = new FileReader();
        reader.onload = (e: any) => this.previewImages.push(e.target.result);
        reader.readAsDataURL(file);
      }
    }
    input.value = '';
  }

  removeImage(index: number): void {
    this.selectedFiles.splice(index, 1);
    this.previewImages.splice(index, 1);
  }

  clearAllImages(): void {
    this.selectedFiles = [];
    this.previewImages = [];
  }

  async uploadImage(file: File): Promise<string> {
    const storage = getStorage();
    const fileRef = ref(storage, `property-images/${uuidv4()}_${file.name}`);
    const snapshot = await uploadBytes(fileRef, file);
    return getDownloadURL(snapshot.ref);
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (!form.valid) return;

    try {
      const imageUrls: string[] = [];

      if (this.selectedFiles.length > 0) {
        for (const file of this.selectedFiles) {
          const url = await this.uploadImage(file);
          imageUrls.push(url);
        }
      } else {
        // صورة افتراضية إذا ما تم رفع أي صورة
        imageUrls.push('https://via.placeholder.com/400x300?text=No+Image');
      }

      const propertyData = {
        ...form.value,
        country: this.country,
        zipCode: this.zipCode,
        state: this.state,
        city: this.city,
        images: imageUrls,
        status: 'pending', // حالة الإعلان
        createdAt: serverTimestamp()
      };

      const propertiesRef = collection(this.firestore, 'properties');
      await addDoc(propertiesRef, propertyData);

      alert('تم إضافة العقار بنجاح');
      form.resetForm();
      this.clearAllImages();
    } catch (error) {
      console.error('خطأ أثناء إضافة العقار:', error);
    }
  }
}