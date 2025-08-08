import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { Firestore, collection, addDoc } from '@angular/fire/firestore';
import { getStorage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { CommonModule } from '@angular/common';
import { FileSelectEvent, FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from "primeng/button";
import { ProgressBarModule } from 'primeng/progressbar';
import { BadgeModule } from 'primeng/badge';
import { MessageService } from 'primeng/api';
import { TranslateModule } from '@ngx-translate/core';
@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule,TranslateModule, FormsModule, FileUploadModule, ToastModule, ButtonModule, ProgressBarModule, BadgeModule],
  providers: [MessageService],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {

  previewImages: string[] = [];
  totalSize: number = 0;
  totalSizePercent: number = 0;
  event: any;

  selectedFiles: File[] = [];

  onSelectedFiles(event: any): void {
    if (!event.files) return;

    for (let file of event.files) {
      this.totalSize += file.size || 0;
    }

    this.updateTotalSizePercent();
  }

  onTemplatedUpload(): void {
    this.totalSize = 0;
    this.totalSizePercent = 0;
  }

  updateTotalSizePercent(): void {
    // 1MB = 1,000,000 bytes
    this.totalSizePercent = this.totalSize / 1000000 * 100;
  }

  choose(event: Event, chooseCallback: Function): void {
    if (chooseCallback) {
      chooseCallback(); 
    }
  }

  uploadEvent(uploadCallback: Function): void {
    if (uploadCallback) {
      uploadCallback();
    }
  }

  onRemoveTemplatingFile(event: Event, file: any, removeFileCallback: Function, index: number): void {
    this.totalSize -= file.size;
    this.updateTotalSizePercent();
    removeFileCallback(index);
  }

  formatSize(bytes: number): string {
    if (bytes === 0) {
      return '0 Bytes';
    }

    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    const size = parseFloat((bytes / Math.pow(k, i)).toFixed(2));

    return `${size} ${sizes[i]}`;
  }


  zipCode: string = '';
  country: string = ''; // 'us' or 'de'
  state: string = '';
  city: string = '';


  index: any;
  constructor(private http: HttpClient, private firestore: Firestore) { }

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

  async uploadImage(file: File): Promise<string> {
    const storage = getStorage();
    const fileRef = ref(storage, `property-images/${Date.now()}_${file.name}`);
    const snapshot = await uploadBytes(fileRef, file);
    return getDownloadURL(snapshot.ref);
  }


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const newFiles = Array.from(input.files);

      for (const file of newFiles) {
        this.selectedFiles.push(file);

        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewImages.push(e.target.result);
        };
        reader.readAsDataURL(file);
      }
    }
    // Reset the input value to allow re-selection of the same file
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

  onSubmit(form: NgForm): void {
    if (form.valid) {
      const formData = new FormData();

      // Add form fields
      Object.keys(form.value).forEach(key => {
        formData.append(key, form.value[key]);
      });

      // Add images
      this.selectedFiles.forEach((file, index) => {
        formData.append('propertyImages[]', file, file.name);
      });

      // Submit to server
      this.http.post('/api/properties', formData).subscribe(
        response => {
          console.log('Form submitted successfully', response);
        },
        error => {
          console.error('Form submission failed', error);
        }
      );
    }
  }



}