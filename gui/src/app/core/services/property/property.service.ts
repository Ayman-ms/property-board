import { HttpClient, HttpParams,HttpHeaders} from '@angular/common/http';
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

  // 2. get properties with filters
  getProperties(filters: any = {}): Observable<any> {
    let params = new HttpParams();

    // Adding filter parameters dynamically if they exist
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

getMyProperties(): Observable<any[]> {
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });
  return this.http.get<any[]>(`${ApiEndpoints.properties.base}/my`, { headers });
}
  async addProperty() {

  }


  async getPropertyImages() {

  }

  deleteProperty(id: number | string): Observable<any> {
    return this.http.delete(`${ApiEndpoints.properties.base}/${id}`);
  }

  async updatePropertyStatus() {

  }
}