import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { Firestore, collection, addDoc, serverTimestamp } from '@angular/fire/firestore';
import { getStorage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { v4 as uuidv4 } from 'uuid';
import { AppMessageService } from '../../../core/services/message/message.service';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {
  currentStep = 1;
  totalSteps = 5;
propertyData = {
    title: '',
    description: '',
    shortDescription: '',
    propertyTypeId: null,
    listingType: 'rent',
    price: 0,
    areaSqm: 0,
    bedrooms: 0,
    bathrooms: 0,
    floorNumber: 0,
    totalFloors: 0,
    yearBuilt: new Date().getFullYear(),
    hasParking: false,
    hasBalcony: false,
    hasGarden: false,
    hasElevator: false,
    hasGarage: false,
    isFurnished: false,
    isNegotiable: false,
    street: '',
    zipCode: '',
    city: '',
    state: '',
    country: 'de'
  };
  // Form Data
  listingType = 'rent';
  propertyType = 'apartment';
  country = 'de';
  zipCode = '';
  state = '';
  city = '';
  
  // Media
  previewImages: string[] = [];
  selectedFiles: File[] = [];

  constructor(private http: HttpClient, private firestore: Firestore, private msg: AppMessageService) {}

  // التنقل بين الخطوات
  nextStep() { if (this.currentStep < this.totalSteps) this.currentStep++; }
  prevStep() { if (this.currentStep > 1) this.currentStep--; }

  lookupZip() {
    if (!this.zipCode || this.zipCode.length < 4) return;
    const url = `https://api.zippopotam.us/${this.country}/${this.zipCode}`;
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const place = res.places[0];
        this.city = place['place name'];
        this.state = place['state'];
      },
      error: () => { this.city = ''; this.state = ''; }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const newFiles = Array.from(input.files);
      newFiles.forEach(file => {
        this.selectedFiles.push(file);
        const reader = new FileReader();
        reader.onload = (e: any) => this.previewImages.push(e.target.result);
        reader.readAsDataURL(file);
      });
    }
  }

  async onSubmit(form: NgForm): Promise<void> {
    if (!form.valid) return;
    try {
      const imageUrls = [];
      for (const file of this.selectedFiles) {
        const storage = getStorage();
        const fileRef = ref(storage, `property-images/${uuidv4()}_${file.name}`);
        const snapshot = await uploadBytes(fileRef, file);
        const url = await getDownloadURL(snapshot.ref);
        imageUrls.push(url);
      }

      const propertyData = {
        ...form.value,
        listingType: this.listingType,
        propertyType: this.propertyType,
        country: this.country,
        city: this.city,
        state: this.state,
        images: imageUrls,
        createdAt: serverTimestamp()
      };

      await addDoc(collection(this.firestore, 'properties'), propertyData);
      this.msg.success('Success', 'Property Added Successfully');
      this.currentStep = 1;
      form.resetForm();
    } catch (error) {
      this.msg.error('Error', String(error));
    }
  }
}