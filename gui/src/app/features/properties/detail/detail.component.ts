import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../../core/services/property/property.service';
import { UserService } from '../../../core/services/user/user.service';
import { switchMap, of } from 'rxjs';
import { ConversationService } from '../../../core/services/conversation/conversation.service';
@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class DetailComponent {
  propertyId: string | null = null;
  ownerId: string | null = null;
  property: any;
  owner: any;
  contactMessage: string = "I'm interested in this property. Please contact me.";

  constructor(private route: ActivatedRoute,
    private propertyService: PropertyService,
    protected userService: UserService,
    private conversationService: ConversationService
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

sendRequest() {
  if (!this.owner || !this.property) return;

  const sellerId = Number(this.owner.userId);
  const propertyId = Number(this.property.propertyId);

  this.conversationService.startConversation(sellerId, propertyId, this.contactMessage).subscribe({
    next: (res) => {
      alert("Request sent successfully!");
      this.contactMessage = "";
    },
    error: (err) => console.error("Error:", err)
  });
}
}