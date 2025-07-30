import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly storageKey = 'darkMode';

  constructor() {}

  initTheme(): void {
    const saved = localStorage.getItem(this.storageKey);
    const isDark = saved === 'true';
    this.applyDarkMode(isDark);
  }

  toggleTheme(): void {
    const isDark = document.body.classList.contains('dark-mode');
    this.applyDarkMode(!isDark);
  }

  private applyDarkMode(enable: boolean): void {
    document.body.classList.toggle('dark-mode', enable);
    localStorage.setItem(this.storageKey, String(enable));
  }

  isDarkMode(): boolean {
    return document.body.classList.contains('dark-mode');
  }
}
