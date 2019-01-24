import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from 'src/app/shared/shared.module';
import { CourseRoutingModule } from './course-routing.module';
import { CourseApiModule } from '../api/course-api.module';
import { CourseListComponent } from './components/course-list/course-list.component';
import { CourseTableComponent } from './components/course-list/course-table/course-table.component';
import { CourseNewComponent } from './components/course-new/course-new.component';
import { CourseEditComponent } from './components/course-edit/course-edit.component';
import { CourseStudentsComponent } from './components/course-students/course-students.component';
import { StudentApiModule } from '../../student/api/student-api.module';
import { CourseStudentEditDialogComponent } from './components/course-students/course-student-edit-dialog/course-student-edit-dialog.component';


@NgModule({
  declarations: [CourseListComponent, CourseTableComponent, CourseNewComponent,  CourseEditComponent,
     CourseStudentsComponent, CourseStudentEditDialogComponent],
  imports: [
    SharedModule,
    CourseRoutingModule,
    CourseApiModule,
    StudentApiModule,
    CommonModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    CourseStudentEditDialogComponent
  ]
})
export class CourseUiModule { }
