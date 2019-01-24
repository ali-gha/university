import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentRoutingModule } from './student-routing.module';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentTableComponent } from './components/student-list/student-table/student-table.component';
import { StudentNewComponent } from './components/student-new/student-new.component';
import { StudentEditComponent } from './components/student-edit/student-edit.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { StudentApiModule } from '../api/student-api.module';

@NgModule({
  declarations: [StudentListComponent, StudentTableComponent, StudentNewComponent, StudentEditComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    StudentRoutingModule,
    SharedModule,
    StudentApiModule
  ]
})
export class StudentUiModule { }
