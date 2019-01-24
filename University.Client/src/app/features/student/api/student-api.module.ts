import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { StudentService } from './services/student.service';
import { UniqueNameValidator } from './asyncValidator/user.validator';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [StudentService, UniqueNameValidator]
})
export class StudentApiModule { }
