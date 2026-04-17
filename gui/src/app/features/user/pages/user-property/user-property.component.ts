import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../../../shared/property-card/property-card.component';
@Component({
  selector: 'app-user-property',
  standalone: true,
  imports: [CommonModule, PropertyCardComponent, FormsModule, RouterModule],
  templateUrl: './user-property.component.html',
  styleUrl: './user-property.component.scss'
})
export class UserPropertyComponent {
myProperties: any[] = []; // المصفوفة التي تعرض عقارات المستخدم

  constructor() {}

  ngOnInit(): void {
    // هنا استدعِ الخدمة لجلب عقارات المستخدم
    // this.loadUserProperties();
  }

  // --- إصلاح الخطأ: تعريف الدوال المفقودة ---

  onEdit(property: any) {
    console.log('سيتم فتح صفحة التعديل للعقار:', property.title);
    // هنا يمكنك التوجيه لصفحة التعديل:
    // this.router.navigate(['/user/edit-property', property.propertyId]);
  }

  onDelete(property: any) {
    if (confirm('هل أنت متأكد من حذف هذا العقار؟')) {
      console.log('جاري حذف العقار رقم:', property.propertyId);
      // هنا تستدعي خدمة الحذف من الـ API
    }
  }
}
