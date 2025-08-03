import { inject } from '@angular/core';
import { httpsCallable } from '@angular/fire/functions';
import { Functions } from '@angular/fire/functions';

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


