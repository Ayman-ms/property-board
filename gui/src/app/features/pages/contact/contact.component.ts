import { Component, inject } from '@angular/core';
import { httpsCallable } from '@angular/fire/functions';
import { Functions } from '@angular/fire/functions';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  imports: [TranslateModule],
    standalone: true,
})
export class ContactComponent {
  private functions = inject(Functions);

  sendMessage() {
    const sendContactEmail = httpsCallable(this.functions, 'sendContactEmail');

    sendContactEmail({
      name: 'John Doe',
      email: 'john@example.com',
      subject: 'Test Subject',
      message: 'This is a test message.'
    })
    .then((res) => console.log('Success!', res))
    .catch((err) => console.error('Error:', err));
  }
}


