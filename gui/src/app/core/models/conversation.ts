export interface Conversation {
    conversation_id: number;
    property_id: number;
    buyer_user_id: number;
    seller_user_id: number;
    status: string;
    created_at: Date;
    updated_at: Date;
}
