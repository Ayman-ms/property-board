import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { AppMessageService } from '../../../core/services/message/message.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ApiEndpoints } from '../../../core/constants/api_endpoints';
import {
  faP, faTree, faPersonShelter, faCouch,
  faMoneyBills, faArrowsUpDown, faWarehouse
} from '@fortawesome/free-solid-svg-icons';
import { Subject, debounceTime, distinctUntilChanged, switchMap, of } from 'rxjs';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule, FontAwesomeModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {

  // ─── Icons ───────────────────────────────────────────
  faParking    = faP;
  faGarden     = faTree;
  faElevator   = faArrowsUpDown;
  faGarage     = faWarehouse;
  faFurnished  = faCouch;
  faNegotiable = faMoneyBills;
  faBalcony    = faPersonShelter;

  // ─── Stepper ─────────────────────────────────────────
  currentStep = 1;
  totalSteps  = 6;
  isLoading   = false;

  // ─── Address Search ──────────────────────────────────
  suggestions  : any[] = [];
  isSearching          = false;
  searchQuery          = '';
  private searchInput$ = new Subject<string>();

  // خريطة تحويل كود الدولة ← اسمها الكامل (للـ Nominatim)
  private countryCodeMap: { [key: string]: string } = {
    'Germany'       : 'de',
    'United States' : 'us'
  };

  // ─── Property Data ───────────────────────────────────
  propertyData = {
    title           : '',
    description     : '',
    shortDescription: '',
    propertyTypeId  : 0,
    listingType     : 'rent',
    price           : null as number | null,
    areaSqm         : null as number | null,
    bedrooms        : null as number | null,
    bathrooms       : null as number | null,
    floorNumber     : null as number | null,
    totalFloors     : null as number | null,
    yearBuilt       : new Date().getFullYear(),
    hasParking      : false,
    hasBalcony      : false,
    hasGarden       : false,
    hasElevator     : false,
    hasGarage       : false,
    isFurnished     : false,
    isNegotiable    : false,
    street          : '',
    postCode        : '',
    city            : '',
    state           : '',
    country         : 'Germany',   // ✅ القيمة الكاملة كما تتوقعها قاعدة البيانات
    currency        : 'EUR',
    latitude        : null as number | null,
    longitude       : null as number | null,
  };

  // ─── Media ───────────────────────────────────────────
  previewImages: string[] = [];
  selectedFiles: File[]   = [];

  constructor(
    private http: HttpClient,
    private msg : AppMessageService
  ) {
    // Debounce البحث عن العناوين - ينتظر 500ms بعد آخر حرف
    this.searchInput$.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(query => {
        if (query.length < 3) {
          this.suggestions = [];
          return of([]);
        }
        this.isSearching = true;

        // تحويل اسم الدولة الكامل إلى كود للـ Nominatim
        const countryCode = this.countryCodeMap[this.propertyData.country] || 'de';
        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}&countrycodes=${countryCode}&limit=5&addressdetails=1`;

        return this.http.get<any[]>(url);
      })
    ).subscribe({
      next: (data) => { this.suggestions = data; this.isSearching = false; },
      error: ()     => { this.isSearching = false; }
    });
  }

  // ─── Address Methods ─────────────────────────────────
  onSearchAddress(event: any) {
    this.searchQuery = event.target.value;
    this.searchInput$.next(this.searchQuery);
  }

  selectAddress(item: any) {
    const addr = item.address;

    // تحويل كود الدولة القادم من Nominatim إلى الاسم الكامل
    const countryFullNameMap: { [key: string]: string } = {
      'de': 'Germany',
      'us': 'United States'
    };

    this.propertyData.postCode  = addr.postcode || '';
    this.propertyData.city      = addr.city || addr.town || addr.village || addr.suburb || '';
    this.propertyData.state     = addr.state || '';
    this.propertyData.street    = addr.road ? `${addr.road} ${addr.house_number || ''}`.trim() : '';
    this.propertyData.country   = countryFullNameMap[addr.country_code] || this.propertyData.country;
    this.propertyData.latitude  = parseFloat(item.lat) || null;
    this.propertyData.longitude = parseFloat(item.lon) || null;
    this.searchQuery             = item.display_name;
    this.suggestions             = [];
  }

  closeSuggestions() {
    setTimeout(() => this.suggestions = [], 200);
  }

  // ─── Stepper Navigation ───────────────────────────────
  nextStep() { if (this.currentStep < this.totalSteps) this.currentStep++; }
  prevStep()  { if (this.currentStep > 1) this.currentStep--; }

  // ─── File Upload ──────────────────────────────────────
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;
    Array.from(input.files).forEach(file => {
      this.selectedFiles.push(file);
      const reader = new FileReader();
      reader.onload = (e: any) => this.previewImages.push(e.target.result);
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number) {
    this.previewImages.splice(index, 1);
    this.selectedFiles.splice(index, 1);
  }

  // ─── Submit ───────────────────────────────────────────
  async onSubmit(form: NgForm): Promise<void> {
    if (!form.valid) {
      this.msg.error('Error', 'Please fill all required fields');
      return;
    }

    this.isLoading = true;

    try {
      const token = localStorage.getItem('token');
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
        'Content-Type' : 'application/json'
      });

      // الخطوة 1: إنشاء العقار في قاعدة البيانات
      const propertyResponse = await this.http
        .post<any>(`${ApiEndpoints.properties.base}`, this.propertyData, { headers })
        .toPromise();

      const propertyId = propertyResponse?.id;

      // الخطوة 2: رفع الصور إذا وجدت
      if (this.selectedFiles.length > 0 && propertyId) {
        const formData = new FormData();
        this.selectedFiles.forEach(file => formData.append('files', file));

        const uploadHeaders = new HttpHeaders({
          'Authorization': `Bearer ${token}`
          // لا تضع Content-Type هنا، المتصفح يضيفه تلقائياً مع boundary
        });

        await this.http
          .post(
            `${ApiEndpoints.properties.base}/${propertyId}/media`,
            formData,
            { headers: uploadHeaders }
          )
          .toPromise();
      }

      this.msg.success('Success', 'Property Added Successfully');
      this.resetAll(form);

    } catch (error: any) {
      console.error(error);
      this.msg.error('Error', error?.error?.message || 'Failed to save property');
    } finally {
      this.isLoading = false;
    }
  }

  // ─── Reset ────────────────────────────────────────────
  resetAll(form: NgForm) {
    this.currentStep              = 1;
    this.searchQuery              = '';
    this.suggestions              = [];
    this.previewImages            = [];
    this.selectedFiles            = [];
    this.propertyData.listingType = 'rent';
    this.propertyData.country     = 'Germany';
    form.resetForm();
  }
}