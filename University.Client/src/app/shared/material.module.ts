import { NgModule } from '@angular/core';
import {
    MatButtonModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatSelectModule,
    MatDialogModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
} from '@angular/material';

const materialModules = [
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatSelectModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
];

@NgModule({
    imports: [
        materialModules
    ],
    exports: [
        materialModules
    ],
    providers: [
    ]
})
export class MaterialModule { }
