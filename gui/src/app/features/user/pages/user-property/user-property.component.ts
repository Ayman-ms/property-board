import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../../../shared/property-card/property-card.component';
import { PropertyService } from '../../../../core/services/property/property.service';
@Component({
  selector: 'app-user-property',
  standalone: true,
  imports: [CommonModule, PropertyCardComponent, FormsModule, RouterModule],
  templateUrl: './user-property.component.html',
  styleUrl: './user-property.component.scss'
})
export class UserPropertyComponent {
  myProperties: any[] = [];

  constructor(private propertyService: PropertyService) { }

  ngOnInit(): void {
    this.loadUserProperties();
  }

  
loadUserProperties() {
  this.propertyService.getMyProperties().subscribe({
    next: (res: any[]) => {
      this.myProperties = res;
    },
    error: (err) => console.error(err)
  });
}

  onEdit(property: any) {
    console.log('open:', property.title);
    // redirect to edit page
    // this.router.navigate(['/user/edit-property', property.propertyId]);
  }

  onDelete(property: any) {
    if (confirm('هل أنت متأكد من حذف هذا العقار؟')) {
      console.log('جاري حذف العقار رقم:', property.propertyId);
      // هنا تستدعي خدمة الحذف من الـ API
    }
  }
}
