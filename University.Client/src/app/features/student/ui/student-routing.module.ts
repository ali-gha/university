import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentNewComponent } from './components/student-new/student-new.component';
import { StudentEditComponent } from './components/student-edit/student-edit.component';

export const routes: Routes = [
  {
    path: '', redirectTo: 'student-list', pathMatch: 'full',
  },
  {
    path: 'student-list', children: [
      { path: '', component: StudentListComponent },
      { path: 'new', component: StudentNewComponent },
      { path: 'edit/:id', component: StudentEditComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
