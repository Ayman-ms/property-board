export interface User {
    user_id: number;
    first_name: string;
    last_name: string;
    email: string;
    password_hash: string;
    phone: string;
    user_type: string;
    language: string;
    is_active: boolean;
    is_verified: boolean;
    created_at: Date;
    updated_at: Date;
    last_login: Date;
}