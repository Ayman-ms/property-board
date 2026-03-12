import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../constants/api_endpoints';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private http: HttpClient) { }

  getAllConversations() {
    return this.http.get(ApiEndpoints.conversation.base);
  }

  getConversationsByUserId(userId: number) {
    return this.http.get(`${ApiEndpoints.conversation.base}?userId=${userId}`);
  }

// conversation.service.ts
startConversation(sellerId: number, propertyId: number, message: string) {
  return this.http.post(`${ApiEndpoints.conversation.base}/start`, {
    sellerId: sellerId,
    propertyId: propertyId,
    messageText: message
  });
}

  sendMessage(conversationId: number, message: string) {
    return this.http.post(ApiEndpoints.conversation.send, {
      conversationId,
      message
    });
  }

}
