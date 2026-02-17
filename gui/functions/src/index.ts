import * as functions from 'firebase-functions';
import * as admin from 'firebase-admin';
import fetch from 'node-fetch';

admin.initializeApp();

// Trigger when a new ad is created
export const onNewAdCreated = functions.firestore
  .document('ads/{adId}')
  .onCreate(async (snap, context) => {
    const adData = snap.data();

    // 1) Save admin notification in Firestore
    await admin.firestore().collection('admin_notifications').add({
      message: `New announcement: ${adData.title}`,
      adId: context.params.adId,
      createdAt: admin.firestore.FieldValue.serverTimestamp(),
      isRead: false
    });

    // 2) Send email via SendGrid
    const sendGridKey = functions.config().sendgrid.key;
    const emailBody = {
      personalizations: [
        {
          to: [{ email: 'admin@example.com' }],
          subject: `New announcement: ${adData.title}`
        }
      ],
      from: { email: 'no-reply@property-board.com' },
      content: [
        {
          type: 'text/plain',
          value: `New announcement added:\n\nTitle: ${adData.title}\nDescription: ${adData.description}`
        }
      ]
    };

    try {
      const response = await fetch('https://api.sendgrid.com/v3/mail/send', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${sendGridKey}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(emailBody)
      });

      if (!response.ok) {
        throw new Error(`SendGrid API error: ${response.statusText}`);
      }

      console.log('Email sent successfully.');
    } catch (error) {
      console.error('Error sending email:', error);
    }
  });
