import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CourseStudentModel } from 'src/app/features/course/api/models/course-student.model';
import { CourseService } from 'src/app/features/course/api/services/course.service';
import { CourseForUpdateModel } from 'src/app/features/course/api/models/course-for-update.model';
import { CourseStudentForUpdateModel } from 'src/app/features/course/api/models/course-student-for-update.model';

@Component({
  selector: 'app-course-student-edit-dialog',
  templateUrl: './course-student-edit-dialog.component.html',
  styleUrls: ['./course-student-edit-dialog.component.scss']
})
export class CourseStudentEditDialogComponent implements OnInit {
  gpaForm: FormGroup;
  courseStudent: CourseForUpdateModel;
  constructor(private courseService: CourseService, private dialogref: MatDialogRef<CourseStudentEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CourseStudentModel) { }

  ngOnInit() {
    this.gpaForm = new FormGroup({
      gpa: new FormControl(this.data.gpa, [Validators.required]),
    });
    this.courseService.getCourseById(this.data.courseId)
      .subscribe((response) => {
        this.courseStudent = new CourseForUpdateModel();
        this.courseStudent.name = response.name;
        this.courseStudent.teacherName = response.teacherName;
        this.courseStudent.location = response.location;
        this.courseStudent.students = response.students.map<CourseStudentForUpdateModel>(x =>  {
          const courseStudentForUpdate = new CourseStudentForUpdateModel();
          courseStudentForUpdate.studentId = x.studentId;
          courseStudentForUpdate.gpa = x.gpa;
          return courseStudentForUpdate;
        });
      });
  }
  onSave() {
    if (!this.gpaForm.valid) {
      this.gpaForm.controls['gpa'].markAsTouched();
      return;
    }

    const value = this.gpaForm.value;
    const student = this.courseStudent.students.filter(x => x.studentId === this.data.studentId)[0];
    student.gpa = value.gpa;
    this.courseService.updateCourse(this.data.courseId, this.courseStudent).subscribe(() => {
      this.dialogref.close(true);
    });
  }
  onCancel(): void {
    this.dialogref.close(false);
  }
}
