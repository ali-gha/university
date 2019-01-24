import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CourseListComponent } from './components/course-list/course-list.component';
import { CourseNewComponent } from './components/course-new/course-new.component';
import { CourseEditComponent } from './components/course-edit/course-edit.component';

export const routes: Routes = [
  {
    path: '', redirectTo: 'course-list', pathMatch: 'full',
  },
  {
    path: 'course-list', children: [
      { path: '', component: CourseListComponent },
      { path: 'new', component: CourseNewComponent },
      { path: 'edit/:id', component: CourseEditComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CourseRoutingModule { }
