export interface PropertyMedia {
  media_id: number;
  property_id: number;
  media_type: string;
  file_url: string;
  file_name: string;
  display_order: number;
  is_primary: boolean;
  alt_text: string;
  created_at: Date;
}
