import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./features/auth/login/login.component";
import { NavbarComponent } from "./shared/components/navbar/navbar.component";
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { TranslateService } from '@ngx-translate/core';
import { I18nService } from './core/services/i18n/i18n.service'; // استيراد الخدمة

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent, NavbarComponent, ButtonModule, CardModule, InputTextModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'gui';

  constructor(
    private translate: TranslateService,
    private i18nService: I18nService // حقن الخدمة
  ) {
    const savedLang = this.i18nService.getCurrentLang(); // الحصول على اللغة المحفوظة
    translate.setDefaultLang(savedLang); // تحديد اللغة الافتراضية
    translate.use(savedLang); // استخدام اللغة المحفوظة
  }

  switchLang(lang: string) {
    this.translate.use(lang);
    this.i18nService.setLanguage(lang); // حفظ اللغة عند التبديل
  }
}
