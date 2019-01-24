import { CourseForUpdateModel } from 'src/app/features/course/api/models/course-for-update.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar, MatTableDataSource } from '@angular/material';

import { CourseService } from '../../../api/services/course.service';
import { StudentService } from 'src/app/features/student/api/services/student.service';
import { CourseStudentModel } from '../../../api/models/course-student.model';
import { CourseStudentForUpdateModel } from '../../../api/models/course-student-for-update.model';
import { StudentModel } from 'src/app/shared/base-model/student.model';

@Component({
  selector: 'app-course-edit',
  templateUrl: './course-edit.component.html',
  styleUrls: ['./course-edit.component.scss']
})
export class CourseEditComponent implements OnInit {
  course: CourseForUpdateModel;
  id: number;
  courseEditForm: FormGroup;
  dataSource: MatTableDataSource<StudentModel>;
  displayedColumns: string[] = ['studentName', 'age', 'delete'];
  courseStudents: CourseStudentModel[];
  students: StudentModel[];

  constructor(private route: ActivatedRoute, private courseService: CourseService, private studentService: StudentService,
    private location: Location, private snakBar: MatSnackBar) { }

  ngOnInit() {
    this.courseEditForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      teacherName: new FormControl(null, [Validators.required]),
      location: new FormControl(null, [Validators.required]),
      studentSelect: new FormControl(null),
    });

    this.id = this.route.params['value'].id;
    this.courseService.getCourseById(this.id)
      .subscribe((response) => {
        this.courseEditForm.setValue({
          name: response.name,
          teacherName: response.teacherName,
          location: response.location,
          studentSelect: null
        });
        this.dataSource = new MatTableDataSource(response.students.map(res => {
          const studentModel = new StudentModel();
          studentModel.studentId = res.studentId,
          studentModel.studentName = res.studentName,
          studentModel.age = res.age;
          studentModel.gpa = res.gpa;
          return studentModel;
        }));
        this.setObservables();
      });
    this.loadStudents();
  }
  loadStudents(): any {
    this.studentService.getAllStudents().subscribe((res) => {
      this.students = res;
    });
  }
  setObservables() {
    this.courseEditForm.controls['studentSelect']
      .valueChanges
      .subscribe(id => {
        const data = this.dataSource.data;
        const student = this.students.filter(x => x.studentId === id)[0];
        if (data.filter(x => x.studentId === id).length > 0) {
          this.snakBar.open(student.studentName + ' already added', '', {
            duration: 1000,
          });
        } else {
          data.push({studentId: student.studentId, studentName: student.studentName, age: student.age, gpa: student.gpa});
          this.dataSource.data = data;
        }
      });
  }
  onSave() {
    if (!this.courseEditForm.valid) {
      this.courseEditForm.controls['name'].markAsTouched();
      this.courseEditForm.controls['teacherName'].markAsTouched();
      this.courseEditForm.controls['location'].markAsTouched();
      return;
    }

    const value = this.courseEditForm.value;

    this.course = new CourseForUpdateModel();
    this.course.name = value.name;
    this.course.teacherName = value.teacherName;
    this.course.location = value.location;
    this.course.students = this.dataSource.data.map<CourseStudentForUpdateModel>(x =>  {
      const courseStudentForUpdate = new CourseStudentForUpdateModel();
      courseStudentForUpdate.studentId = x.studentId;
      courseStudentForUpdate.name = x.studentName;
      courseStudentForUpdate.age = x.age;
      courseStudentForUpdate.gpa = x.gpa;
      return courseStudentForUpdate;
    });
    this.courseService.updateCourse(this.id, this.course).subscribe(() => {
      this.snakBar.open(this.course.name + ' has been edited', '', {
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
