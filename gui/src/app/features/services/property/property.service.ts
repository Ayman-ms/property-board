import { Injectable } from '@angular/core';
import { Firestore, collection, addDoc, getDocs, query, where, doc, deleteDoc, updateDoc, Timestamp } from '@angular/fire/firestore';
import { v4 as uuidv4 } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  constructor(private firestore: Firestore) {}

  /**
   * إضافة إعلان جديد
   * @param propertyData بيانات الإعلان
   * @param images مصفوفة صور (Base64 أو URL)
   */
  async addProperty(propertyData: any, images: string[]) {
    const propertiesCol = collection(this.firestore, 'properties');

    // إضافة الإعلان
    const propertyRef = await addDoc(propertiesCol, {
      ...propertyData,
      status: 'pending', // حالة الإعلان عند الإنشاء
      createdAt: Timestamp.now()
    });

    // إذا لم توجد صور، نضيف صورة افتراضية
    if (images.length === 0) {
      images.push('https://via.placeholder.com/600x400?text=No+Image');
    }

    // حفظ الصور في كولكشن منفصل
    const imagesCol = collection(this.firestore, 'propertyImages');
    for (const img of images) {
      await addDoc(imagesCol, {
        propertyId: propertyRef.id,
        imageUrl: img,
        createdAt: Timestamp.now()
      });
    }

    return propertyRef.id;
  }

  /**
   * جلب جميع الإعلانات
   */
  async getAllProperties() {
    const q = query(collection(this.firestore, 'properties'), where('status', '==', 'active'));
    const snapshot = await getDocs(q);

    return snapshot.docs.map(doc => ({
      id: doc.id,
      ...doc.data()
    }));
  }

  /**
   * جلب صور إعلان معين
   * @param propertyId معرف الإعلان
   */
  async getPropertyImages(propertyId: string) {
    const q = query(collection(this.firestore, 'propertyImages'), where('propertyId', '==', propertyId));
    const snapshot = await getDocs(q);

    return snapshot.docs.map(doc => ({
      id: doc.id,
      ...doc.data()
    }));
  }

  /**
   * حذف إعلان وصوره
   */
  async deleteProperty(propertyId: string) {
    // حذف الإعلان
    await deleteDoc(doc(this.firestore, 'properties', propertyId));

    // حذف الصور المرتبطة
    const images = await this.getPropertyImages(propertyId);
    for (const img of images) {
      await deleteDoc(doc(this.firestore, 'propertyImages', img.id));
    }
  }

  /**
   * تحديث حالة الإعلان
   */
  async updatePropertyStatus(propertyId: string, status: 'active' | 'pending') {
    await updateDoc(doc(this.firestore, 'properties', propertyId), {
      status
    });
  }
}