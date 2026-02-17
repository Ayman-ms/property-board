export interface PropertyView {
    view_id: number;
    property_id: number;
    user_id: number | null;
    ip_address: string;
    user_agent: string;
    viewed_at: Date;
}