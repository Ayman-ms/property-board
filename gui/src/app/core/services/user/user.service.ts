import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiEndpoints } from '../../constants/api_endpoints';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getUsers(): Observable<any[]> {
    return this.http.get<any>(ApiEndpoints.users.base).pipe(
      map(res => res.data.data)
    );
  }

  getUserById(user_id: string): Observable<User> {
    return this.http.get<User>(`${ApiEndpoints.users.base}/${user_id}`);
  }

  async createUser(user: User): Promise<void> {
    await this.http.post(ApiEndpoints.users.base, user).toPromise();
  }

  updateUser(user_id: string, data: Partial<User>): Observable<any> {
    return this.http.put(`${ApiEndpoints.users.base}/${user_id}`, data);
  }

  deleteUser(user_id: string): Observable<any> {
    return this.http.delete(`${ApiEndpoints.users.base}/${user_id}`);
  }

}