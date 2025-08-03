import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule , NgForm } from '@angular/forms';
import { Firestore, collection, addDoc } from '@angular/fire/firestore';
import { getStorage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {
  zipCode: string = '';
  country: string = ''; // 'us' or 'de'
  state: string = '';
  city: string = '';

previewImages: string[] = [];
  selectedFiles: File[] = [];
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
        alert('ZIP code not found for ' + this.country.toUpperCase());
      }
    });
  }

  onFileSelected(event: any) {
    const files = Array.from(event.target.files) as File[];
    this.selectedFiles = files;

    this.previewImages = [];
    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = () => this.previewImages.push(reader.result as string);
      reader.readAsDataURL(file);
    });
  }

  async uploadImage(file: File): Promise<string> {
    const storage = getStorage();
    const fileRef = ref(storage, `property-images/${Date.now()}_${file.name}`);
    const snapshot = await uploadBytes(fileRef, file);
    return getDownloadURL(snapshot.ref);
  }

  async onSubmit(form: NgForm) {
    const data = form.value;

    // تحميل الصور أو تعيين صورة افتراضية
    let imageUrls: string[] = [];

    if (this.selectedFiles.length > 0) {
      imageUrls = await Promise.all(this.selectedFiles.map(file => this.uploadImage(file)));
    } else {
      imageUrls = ['https://via.placeholder.com/800x600?text=No+Image']; // صورة افتراضية
    }

    // إعداد بيانات العقار
    const property = {
      ...data,
      state: this.state,
      city: this.city,
      country: this.country,
      zipCode: this.zipCode,
      images: imageUrls,
      createdAt: new Date()
    };

    // حفظ في Firestore
    await addDoc(collection(this.firestore, 'properties'), property);
    alert('تم حفظ العقار بنجاح');
    form.resetForm();
    this.previewImages = [];
    this.selectedFiles = [];
  }
}