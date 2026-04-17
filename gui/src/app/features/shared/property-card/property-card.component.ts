import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PropertyService } from '../../../core/services/property/property.service';

@Component({
  selector: 'app-property-card',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslateModule],
  templateUrl: './property-card.component.html',
  styleUrl: './property-card.component.scss'
})
export class PropertyCardComponent {
    constructor(private propertyService: PropertyService,
    private route: ActivatedRoute
  ) { }
propertyTypes: any[] = [];


  @Input() property: any; // يستلم بيانات عقار واحد فقط
  @Input() viewMode: 'grid' | 'list' = 'grid';
  @Input() context: 'visitor' | 'owner' | 'admin' = 'visitor';

  // الأحداث التي يرسلها للأب
  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();

   ngOnInit(): void {
    this.loadPropertyTypes();
  }

  loadPropertyTypes() {
    this.propertyService.getPropertyTypes().subscribe(res => {
      this.propertyTypes = res;
    });
  }

    onTypeChange(typeId: number) {
    // this.filterModel.propertyTypeId = typeId;
    // يمكن استدعاء applyFilters هنا فوراً أو انتظار ضغط زر Apply
  }
  }
