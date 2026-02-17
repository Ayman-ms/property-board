export interface Notification {
    notification_id: number;
    user_id: number;
    type: string;
    title: string;
    message: string;
    related_id: number;
    is_read: boolean;
    created_at: Date;
}