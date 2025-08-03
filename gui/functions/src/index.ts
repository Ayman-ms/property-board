import * as functions from 'firebase-functions';
import * as nodemailer from 'nodemailer';

// Setting the sending email
const transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
        user: functions.config().gmail.user,
        pass: functions.config().gmail.pass
    }
});

// Function called from Angular
export const sendContactEmail = functions.https.onCall(async (data, context) => {
    const { name, email, subject, message } = data;

    const mailOptions = {
        from: email,
        to: 'info@i.com',
        subject: `[Contact Form] ${subject}`,
        text: `From: ${name} <${email}>\n\n${message}`
    };

    try {
        await transporter.sendMail(mailOptions);
        return { success: true };
    } catch (error) {
        console.error('Error sending email:', error);
        throw new functions.https.HttpsError('internal', 'Unable to send email');
    }
});
