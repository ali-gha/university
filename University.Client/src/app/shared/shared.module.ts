import { NgModule } from '@angular/core';
import { MaterialModule } from './material.module';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DialogOverviewComponent } from './dialog-overview/dialog-overview.component';
import { GenericService } from './services/generic.service';

@NgModule({
  declarations: [DialogOverviewComponent],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    RouterModule,
    ReactiveFormsModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    RouterModule,
    ReactiveFormsModule
  ],
  providers: [
    GenericService
  ],
  entryComponents: [
    DialogOverviewComponent
  ]
})
export class SharedModule { }
