import { NgModule } from '@angular/core';

import { SharedModule } from 'src/app/shared/shared.module';
import { CourseService } from './services/course.service';

@NgModule({
  declarations: [],
  imports: [
    SharedModule
  ],
  exports: [],
  providers: [CourseService]
})
export class CourseApiModule { }
