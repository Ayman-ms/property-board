import { Routes } from '@angular/router';
import { AddComponent } from './add/add.component';
import { DetailComponent } from './detail/detail.component';
import { EditComponent } from './edit/edit.component';
import { ListComponent } from './list/list.component';


export const propertyRoutes: Routes = [
  { path: 'add', component: AddComponent },
  { path: 'detail', component: DetailComponent },
  { path: 'edit', component: EditComponent },
  { path: 'list', component: ListComponent }

];
