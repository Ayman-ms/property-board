import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PropertyService } from '../../../core/services/property/property.service';
import { UserService } from '../../../core/services/user/user.service';
import { switchMap, of } from 'rxjs';
@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [CommonModule],
  providers: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class DetailComponent {
  propertyId: string | null = null;
  ownerId: string | null = null;
  property: any;
  owner: any;
  constructor(private route: ActivatedRoute,
    private propertyService: PropertyService,
    protected userService: UserService
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.fetchPropertyDetails(id);
      }
    });
  }

  fetchPropertyDetails(id: string) {
  this.propertyService.getPropertyById(id).pipe(
    switchMap(propertyData => {
      this.property = propertyData;
      console.log('Property loaded:', this.property);

      // إذا وجدنا userId، ننتقل لطلب بيانات المستخدم
      if (this.property.userId) {
        return this.userService.getUserById(this.property.userId);
      } else {
        // إذا لم يوجد، نرسل "لاشيء" لإيقاف التدفق
        return of(null);
      }
    })
  ).subscribe({
    next: (response: any) => {
      if (response && response.isSuccess) {
        this.owner = response.data;
      }
      console.info('Owner details extracted:', this.owner);
    },
    error: (error) => {
      console.error('Error in sequence:', error);
    }
  });
}

}