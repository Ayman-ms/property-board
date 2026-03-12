import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../constants/api_endpoints';
import { Property } from '../../models/propertie';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  constructor(private http: HttpClient) { }

  getAllProperties(): Observable<any[]> {
    return this.http.get<any[]>(ApiEndpoints.properties.base);
  }

  
  getPropertyTypes(): Observable<any[]> {
    return this.http.get<any[]>(ApiEndpoints.properties.types);
  }
  // 2. جلب قائمة العقارات مع دعم الفلترة والترقيم
  getProperties(filters: any = {}): Observable<any> {
    let params = new HttpParams();

    // إضافة بارامترات الفلترة ديناميكياً إذا وجدت
    if (filters.page) params = params.set('page', filters.page.toString());
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize.toString());
    if (filters.minPrice) params = params.set('minPrice', filters.minPrice.toString());
    if (filters.maxPrice) params = params.set('maxPrice', filters.maxPrice.toString());
    if (filters.propertyTypeId) params = params.set('propertyTypeId', filters.propertyTypeId.toString());
    if (filters.listingType) params = params.append('listingType', filters.listingType);
    
    return this.http.get<any>(ApiEndpoints.properties.base, { params });
  }
  
  getPropertyById(id: number | string): Observable<Property> {
  return this.http.get<Property>(`${ApiEndpoints.properties.base}/${id}`);
}

  async addProperty() {

  }



  async getPropertyImages() {

  }

  async deleteProperty() {

  }

  async updatePropertyStatus() {

  }
}