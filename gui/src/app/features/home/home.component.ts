import { Component , ViewEncapsulation} from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
 encapsulation: ViewEncapsulation.None
})


export class HomeComponent {
  constructor( public translate: TranslateService ) { }
}
