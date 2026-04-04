import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { Firestore, collection, addDoc, serverTimestamp } from '@angular/fire/firestore';
import { getStorage, ref, uploadBytes, getDownloadURL } from '@angular/fire/storage';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { v4 as uuidv4 } from 'uuid';
import { AppMessageService } from '../../../core/services/message/message.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faP, faTree, faPersonShelter, faCouch, faMoneyBills, faArrowsUpDown, faWarehouse } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule, FontAwesomeModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {

  // Font Awesome Icons
  faParking = faP;
  faGarden = faTree;
  faElevator = faArrowsUpDown;
  faGarage = faWarehouse;
  faFurnished = faCouch;
  faNegotiable = faMoneyBills;
  faBalcony = faPersonShelter;

  // Stepper
  currentStep = 1;
  totalSteps = 5;

  // All property data in one object (no duplicates)
  propertyData = {
    title: '',
    description: '',
    shortDescription: '',
    propertyTypeId: null,
    listingType: 'rent',
    price: null,
    areaSqm: null,
    bedrooms: null,
    bathrooms: null,
    floorNumber: null,
    totalFloors: null,
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

  // Media
  previewImages: string[] = [];
  selectedFiles: File[] = [];

  constructor(
    private http: HttpClient,
    private firestore: Firestore,
    private msg: AppMessageService
  ) {}

  // Stepper Navigation
  nextStep() { if (this.currentStep < this.totalSteps) this.currentStep++; }
  prevStep() { if (this.currentStep > 1) this.currentStep--; }

  // Zip Code Lookup
  lookupZip() {
    if (!this.propertyData.zipCode || this.propertyData.zipCode.length < 4) return;
    const url = `https://api.zippopotam.us/${this.propertyData.country}/${this.propertyData.zipCode}`;
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const place = res.places[0];
        this.propertyData.city = place['place name'];
        this.propertyData.state = place['state'];
      },
      error: () => {
        this.propertyData.city = '';
        this.propertyData.state = '';
      }
    });
  }

  // File Selection
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

  // Submit
  async onSubmit(form: NgForm): Promise<void> {
    if (!form.valid) return;
    try {
      const imageUrls: string[] = [];
      for (const file of this.selectedFiles) {
        const storage = getStorage();
        const fileRef = ref(storage, `property-images/${uuidv4()}_${file.name}`);
        const snapshot = await uploadBytes(fileRef, file);
        const url = await getDownloadURL(snapshot.ref);
        imageUrls.push(url);
      }

      const dataToSubmit = {
        ...this.propertyData,
        images: imageUrls,
        createdAt: serverTimestamp()
      };

      await addDoc(collection(this.firestore, 'properties'), dataToSubmit);
      this.msg.success('Success', 'Property Added Successfully');
      this.currentStep = 1;
      form.resetForm();
      this.propertyData.listingType = 'rent';
      this.previewImages = [];
      this.selectedFiles = [];
    } catch (error) {
      this.msg.error('Error', String(error));
    }
  }
}