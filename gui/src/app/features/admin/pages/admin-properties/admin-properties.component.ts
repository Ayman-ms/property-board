import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../../../shared/property-card/property-card.component';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../../../core/services/property/property.service';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-admin-properties',
  standalone: true,
  imports: [RouterModule, CommonModule, PropertyCardComponent, FormsModule,TranslateModule],
  templateUrl: './admin-properties.component.html',
  styleUrl: './admin-properties.component.scss'
})
export class AdminPropertiesComponent {
  myProperties: any[] = [];
allProperties: any[] = [];
  constructor(
    private router: Router,
    private propertyService: PropertyService
  ) { }

  ngOnInit(): void {
    this.loadUserProperties();
    this.loadAllProperties();
  }
  
  loadUserProperties() {
    this.propertyService.getMyProperties().subscribe({
      next: (res: any[]) => {
        this.myProperties = res;
      },
      error: (err) => console.error(err)
    });
  }

   loadAllProperties() {
    this.propertyService.getProperties().subscribe(res => {
      this.allProperties = res.data || res;
      console.log('All properties:', this.allProperties);
    });
  }

  activeTab: 'mine' | 'all' = 'mine';

onEdit(p: any) {
  this.router.navigate(['/properties/edit', p.propertyId]);
}

onDelete(p: any) {

}

}
