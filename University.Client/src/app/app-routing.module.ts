import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';



const routes: Routes = [
  { path: '', redirectTo: 'university', pathMatch: 'full' },
  {
    path: 'university',  children: [
      { path: 'course', loadChildren: './features/course/ui/course-ui.module#CourseUiModule' },
      { path: 'student', loadChildren: './features/student/ui/student-ui.module#StudentUiModule' }
    ]
  },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
