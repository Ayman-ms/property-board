import { Injectable } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en';
import localeDe from '@angular/common/locales/de';

@Injectable({
  providedIn: 'root'
})
export class I18nService {
  private currentLang: string = 'en';

  constructor() {
    // this will register the locale data for English and German
    registerLocaleData(localeEn);
    registerLocaleData(localeDe);

    const savedLang = localStorage.getItem('lang');
    if (savedLang) {
      this.setLanguage(savedLang);
    } else {
      this.setLanguage('en'); // default language
    }
  }

  setLanguage(lang: string) {
    this.currentLang = lang;
    localStorage.setItem('lang', lang);
    document.documentElement.lang = lang;

    // If you are using translation files like XLF, use here:
    // this.translateService.use(lang); ← if you are using ngx-translate for example
  }

  getCurrentLang(): string {
    return this.currentLang;
  }
}
