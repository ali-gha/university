import { CourseStudentModel } from 'src/app/features/course/api/models/course-student.model';
import {
  Component,
  OnInit,
  Input,
  OnChanges
} from '@angular/core';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { CourseService } from '../../../api/services/course.service';
import { DialogOverviewComponent } from 'src/app/shared/dialog-overview/dialog-overview.component';
import { CourseModel } from '../../../api/models/course.model';
import { CourseStudentEditDialogComponent } from './course-student-edit-dialog/course-student-edit-dialog.component';
import { CourseForUpdateModel } from '../../../api/models/course-for-update.model';
import { CourseStudentForUpdateModel } from '../../../api/models/course-student-for-update.model';

@Component({
  selector: 'app-course-students',
  templateUrl: './course-students.component.html',
  styleUrls: ['./course-students.component.scss']
})
export class CourseStudentsComponent implements OnInit, OnChanges {
  @Input() course: CourseModel;
  dataSource = new MatTableDataSource<CourseStudentModel>();
  displayedColumns: string[] = ['studentName', 'age', 'gpa', 'edit', 'delete'];
  rowClicked = false;
  courseStudent: CourseForUpdateModel;

  constructor(
    private courseService: CourseService,
    private dialog: MatDialog,
  ) {}

  ngOnChanges() {
    if (this.dataSource !== null && this.dataSource.data.length > 0) {
      this.dataSource.data = this.course.students;
    } else {
      this.dataSource = new MatTableDataSource(this.course.students);
    }
  }
  ngOnInit() {}

  onEditCourseStudent(courseStudents: CourseStudentModel) {
    const dialogRef = this.dialog.open(CourseStudentEditDialogComponent, {
      width: '500px',
      data: {
        courseId: this.course.id,
        studentId: courseStudents.studentId,
        gpa: courseStudents.gpa,
        courseName: this.course.name,
        studentName: courseStudents.studentName
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.courseService.getCourseById(this.course.id).subscribe(response => {
          this.dataSource.data = response.students;
        });
      }
    });
  }

  onDeleteCourseStudent(courseStudents: CourseStudentModel) {
    const dialogOverComponent = this.dialog.open(DialogOverviewComponent, {
      width: '250px',
      data: {
        title: 'Student delete',
        content:
          'Are you sure you want to delete the student ' +
          courseStudents.studentName +
          ' ?'
      }
    });
    dialogOverComponent.afterClosed().subscribe(result => {
      if (result) {
        this.course.students = this.course.students.filter(function( obj ) {
          return obj.studentId !== courseStudents.studentId;
      });
          this.courseStudent = new CourseForUpdateModel();
          this.courseStudent.name = this.course.name;
          this.courseStudent.teacherName = this.course.teacherName;
          this.courseStudent.location = this.course.location;
          this.courseStudent.students = this.course.students.map<CourseStudentForUpdateModel>(x => {
            const courseStudentForUpdate = new CourseStudentForUpdateModel();
            courseStudentForUpdate.studentId = x.studentId;
            courseStudentForUpdate.gpa = x.gpa;
            return courseStudentForUpdate;
          });
          this.courseService
            .updateCourse(this.course.id, this.courseStudent)
            .subscribe(() => {
              this.dataSource.data = this.course.students;
            });
      }
    });
  }
}
