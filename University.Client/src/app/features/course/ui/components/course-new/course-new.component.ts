import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar, MatTableDataSource } from '@angular/material';

import { CourseService } from '../../../api/services/course.service';
import { StudentService } from 'src/app/features/student/api/services/student.service';
import { CourseForCreateModel } from '../../../api/models/course-for-create.model';
import { CourseStudentForCreateModel } from '../../../api/models/course-student-for-create.model';
import { CourseStudentModel } from '../../../api/models/course-student.model';
import { StudentModel } from 'src/app/shared/base-model/student.model';

@Component({
  selector: 'app-course-new',
  templateUrl: './course-new.component.html',
  styleUrls: ['./course-new.component.scss']
})
export class CourseNewComponent implements OnInit {
  course: CourseForCreateModel;
  courseNewForm: FormGroup;
  courseStudents: CourseStudentModel[];
  students: StudentModel[];
  dataSource: MatTableDataSource<CourseStudentModel> = new MatTableDataSource();
  displayedColumns: string[] = ['studentName', 'age', 'delete'];

  constructor(private courseService: CourseService, private studentService: StudentService,
    private location: Location, private snakBar: MatSnackBar) { }

  ngOnInit() {
    this.courseNewForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      teacherName: new FormControl(null, [Validators.required]),
      location: new FormControl(null, [Validators.required]),
      studentSelect: new FormControl(null),
    });
    this.loadStudents();
    this.setObservables();
  }
  setObservables() {
    this.courseNewForm.controls['studentSelect']
      .valueChanges
      .subscribe(id => {
        const data = this.dataSource.data;
        const student = this.students.filter(x => x.studentId === id)[0];
        if (data.filter(x => x.studentId === id).length > 0) {
          this.snakBar.open(student.studentName + ' already added', '', {
            duration: 1000,
          });
        } else {
          const courseStudentToAdd = new CourseStudentModel();
          courseStudentToAdd.studentId = student.studentId;
          courseStudentToAdd.studentName = student.studentName;
          courseStudentToAdd.age = student.age;
          data.push(courseStudentToAdd);
          this.dataSource.data = data;
        }
      });
  }
  loadStudents(): any {
    this.studentService.getAllStudents().subscribe((res) => {
      this.students = res;
    });
  }

  onSave() {
    if (!this.courseNewForm.valid) {
      this.courseNewForm.controls['name'].markAsTouched();
      this.courseNewForm.controls['teacherName'].markAsTouched();
      this.courseNewForm.controls['location'].markAsTouched();
      return;
    }

    const value = this.courseNewForm.value;

    this.course = new CourseForCreateModel();
    this.course.name = value.name;
    this.course.teacherName = value.teacherName;
    this.course.location = value.location;
    this.course.students = this.dataSource.data.map<CourseStudentForCreateModel>(x => new CourseStudentForCreateModel(x.studentId));

    this.courseService.addCourse(this.course).subscribe(() => {
      this.snakBar.open(this.course.teacherName + ' has been added', '', {
        duration: 2000,
      });
      this.location.back();
    });
  }
  onDeleteStudent(element) {
    const data = this.dataSource.data;
    const student = data.filter(x => x.studentId === element.studentId)[0];
    const index = data.indexOf(student, 0);
    if (index > -1) {
      data.splice(index, 1);
      this.dataSource.data = data;
    }
  }
  onCancel() {
    this.location.back();
  }
}
