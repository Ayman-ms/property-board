import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ApiEndpoints } from '../../../core/constants/api_endpoints';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  imports: [TranslateModule],
    standalone: true,
})
export class ContactComponent {

constructor(private http:HttpClient) { }

sendEmail(contactForm: any) {
  return this.http.post(ApiEndpoints.contact.base, contactForm)
}


}

