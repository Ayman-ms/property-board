export interface Message {
  message_id: string;
  conversation_id: string;
  sender_user_id: string;
  message_text: string;
  is_read: boolean;
  created_at: Date;
}