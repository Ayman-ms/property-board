import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add.component.html',
  styleUrl: './add.component.scss'
})
export class AddComponent {
  zipCode: string = '';
  country: string = 'de'; // 'us' or 'de'
  state: string = '';
  city: string = '';

  constructor(private http: HttpClient) {}

  lookupZip() {
    if (!this.zipCode || this.zipCode.length < 4) return;

    const url = `https://api.zippopotam.us/${this.country}/${this.zipCode}`;
    this.http.get<any>(url).subscribe({
      next: (res) => {
        const place = res.places[0];
        this.city = place['place name'];
        this.state = place['state'];
      },
      error: () => {
        this.city = '';
        this.state = '';
        alert('ZIP code not found for ' + this.country.toUpperCase());
      }
    });
  }

  onSubmit(form: any) {
  if (form.valid) {
    console.log("Form Submitted:", form.value);
    // هنا أضف ما تريده مثلاً: إرسال البيانات إلى backend
  } else {
    console.warn("Form is invalid");
  }
}
}