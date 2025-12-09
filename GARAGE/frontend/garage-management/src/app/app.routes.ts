import { Routes } from '@angular/router';
import { GarageManagementComponent } from './garage-management/garage-management.component';

export const routes: Routes = [
  { path: '', component: GarageManagementComponent },
  { path: '**', redirectTo: '' }
];
