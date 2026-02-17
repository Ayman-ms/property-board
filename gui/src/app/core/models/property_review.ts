export interface PropertyReview {
  review_id: number;
  property_id: number;
  user_id: number;
  rating: number;
  review_text: string;
  is_approved: boolean;
  created_at: Date;
  updated_at: Date;
}