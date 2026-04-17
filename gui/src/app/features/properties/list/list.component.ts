import { Component, OnInit } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { PropertyService } from '../../../core/services/property/property.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../../shared/property-card/property-card.component';
@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, TranslateModule, FormsModule, RouterModule, PropertyCardComponent],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent implements OnInit {
  properties: any[] = [];
  propertyTypes: any[] = [];
  loading = true;
  filterModel = {
    minPrice: null,
    maxPrice: null,
    propertyTypeId: null,
    listingType: '',
    page: 1,
    pageSize: 10
  };
  viewMode: 'grid' | 'list' = 'grid';
  constructor(private propertyService: PropertyService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.loadPropertyTypes();

    this.route.queryParams.subscribe(params => {
      const typeFromUrl = params['type'];

      this.filterModel.listingType = typeFromUrl ? typeFromUrl : '';

      this.fetchPropertiesData();
    });
  }

  private fetchPropertiesData(): void {
    this.propertyService.getProperties(this.filterModel).subscribe({
      next: (data) => {
        this.properties = data;
        console.log('Data received successfully:', this.properties);
      },
      error: (err) => {
        console.error('Error fetching data from API:', err);
      }
    });
  }

  setListingType(type: 'Rent' | 'Sale' | '') {
    this.filterModel.listingType = type;
    this.filterModel.page = 1; // return to first page when changing listing type
    this.loadProperties();
  }

  setViewMode(mode: 'grid' | 'list') {
    this.viewMode = mode;
  }

  onSortChange(event: any) {
    const sortBy = event.target.value;

    if (sortBy === 'price-low') {
      this.properties.sort((a, b) => a.price - b.price);
    } else if (sortBy === 'price-high') {
      this.properties.sort((a, b) => b.price - a.price);
    } else if (sortBy === 'newest') {
      // افترضنا وجود حقل createdAt أو propertyId للترتيب الزمني
      this.properties.sort((a, b) => b.propertyId - a.propertyId);
    }
  }
  loadRentProperties() {
    this.propertyService.getPropertyTypes().subscribe(res => {
      this.propertyTypes = res;
    });
  }

  loadPropertyTypes() {
    this.propertyService.getPropertyTypes().subscribe(res => {
      this.propertyTypes = res;
    });
  }

  loadProperties() {
    this.propertyService.getProperties(this.filterModel).subscribe(res => {
      this.properties = res.data || res;
    });
  }


  applyFilters() {
    this.propertyService.getProperties(this.filterModel).subscribe(res => {
      this.properties = res.data || res;
    });
  }

  onTypeChange(typeId: number) {
    // this.filterModel.propertyTypeId = typeId;
    // يمكن استدعاء applyFilters هنا فوراً أو انتظار ضغط زر Apply
  }
}
