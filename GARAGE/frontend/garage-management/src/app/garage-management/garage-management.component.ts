import { Component, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { Garage } from '../models/garage.model';
import { GarageService } from '../services/garage.service';

@Component({
  selector: 'app-garage-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './garage-management.component.html',
  styleUrl: './garage-management.component.css'
})
export class GarageManagementComponent implements OnInit {
  allGarages: Garage[] = [];
  displayedGarages: MatTableDataSource<Garage> = new MatTableDataSource<Garage>([]);
  displayedColumns: string[] = ['id', 'name', 'address', 'city', 'phone', 'email'];
  selectedGarages = new FormControl<Garage[]>([]);
  isLoading = false;
  isLoadingTable = false;

  constructor(private garageService: GarageService) {}

  ngOnInit(): void {
    this.loadGaragesFromGovernment();
    this.loadSavedGarages();
  }

  loadGaragesFromGovernment(): void {
    this.isLoading = true;
    this.garageService.getGaragesFromGovernment().subscribe({
      next: (garages) => {
        this.allGarages = garages || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('שגיאה בטעינת מוסכים מהממשלה:', err);
        this.allGarages = [];
        this.isLoading = false;
      }
    });
  }

  loadSavedGarages(): void {
    this.isLoadingTable = true;
    this.garageService.getGarages().subscribe({
      next: (garages) => {
        this.displayedGarages.data = garages || [];
        this.isLoadingTable = false;
      },
      error: (err) => {
        console.error('שגיאה בטעינת מוסכים שמורים:', err);
        this.displayedGarages.data = [];
        this.isLoadingTable = false;
      }
    });
  }

  addGarages(): void {
    const selected = this.selectedGarages.value;
    if (!selected || selected.length === 0) {
      return;
    }

    this.isLoading = true;
    this.garageService.addGarages(selected).subscribe({
      next: () => {
        this.loadSavedGarages();
        this.selectedGarages.setValue([]);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('שגיאה בהוספת מוסכים:', err);
        alert('אירעה שגיאה בהוספת המוסכים. נסה שוב.');
        this.isLoading = false;
      }
    });
  }

  getGarageDisplayName(garage: Garage): string {
    if (!garage) {
      return 'מוסך ללא שם';
    }
    
    const parts: string[] = [];
    if (garage.name?.trim()) {
      parts.push(garage.name.trim());
    }
    if (garage.city?.trim()) {
      parts.push(garage.city.trim());
    }
    
    return parts.length > 0 ? parts.join(' - ') : 'מוסך ללא שם';
  }
}
