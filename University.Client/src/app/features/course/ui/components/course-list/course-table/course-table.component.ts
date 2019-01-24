import { Component, OnInit } from '@angular/core';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';

import { LoaderService } from 'src/app/core/services/loader.service';
import { DialogOverviewComponent } from 'src/app/shared/dialog-overview/dialog-overview.component';
import { CourseModel } from 'src/app/features/course/api/models/course.model';
import { CourseService } from 'src/app/features/course/api/services/course.service';

@Component({
  selector: 'app-course-table',
  templateUrl: './course-table.component.html',
  styleUrls: ['./course-table.component.scss']
})
export class CourseTableComponent implements OnInit {
  dataSource: MatTableDataSource<CourseModel>;
  displayedColumns: string[] = ['id', 'name', 'teacherName', 'location', 'edit', 'delete'];
  rowClicked = false;
  course: CourseModel;
  selectedRowIndex = -1;

  constructor(private courseService: CourseService, private loaderService: LoaderService,
    private dialog: MatDialog, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadCourses();
  }
  onRowClick(row: CourseModel) {
    this.selectedRowIndex = row.id;
    this.courseService.getCourseById(row.id)
        .subscribe((response) => {
          if (response.students.length > 0) {
            this.rowClicked = true;
            this.course = response;
          } else {
            this.rowClicked = false;
          }
        });
  }
  loadCourses() {
    this.loaderService.display(true);
    this.courseService.getAllCourses()
      .subscribe((response: CourseModel[]) => {
        this.loaderService.display(false);
        this.dataSource = new MatTableDataSource(response);
      });
  }

  onEditCourse(course: CourseModel) {
    this.router.navigate(['edit', course.id], { relativeTo: this.route });
  }

  onDeleteCourse(course: CourseModel) {
    const dialogRef = this.dialog.open(DialogOverviewComponent, {
      width: '250px',
      data: {
        title: 'Course delete', content: 'Are you sure you want to delete the course ' + course.name + ' ?',
        isAlert: false
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.courseService.deleteCourse(course.id).subscribe(() => {
          this.rowClicked = false;
          this.loadCourses();
        });
      }
    });
  }
}
