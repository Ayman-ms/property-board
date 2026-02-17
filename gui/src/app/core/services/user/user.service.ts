import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiEndpoints } from '../../constants/api_endpoints';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {}

  getUsers () {
    return this.http.get(ApiEndpoints.users.base).toPromise();
  }

  async createUser(user: User): Promise<void> {
    await this.http.post(ApiEndpoints.users.base, user).toPromise();
  }

  async updateUser(user_id: string, data: Partial<User>): Promise<void> {
    await this.http.put(`${ApiEndpoints.users.base}/${user_id}`, data).toPromise();
  }
}