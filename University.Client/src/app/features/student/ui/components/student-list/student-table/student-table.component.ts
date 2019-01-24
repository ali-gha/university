import { Component, OnInit } from '@angular/core';
import { LoaderService } from 'src/app/core/services/loader.service';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { StudentService } from 'src/app/features/student/api/services/student.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogOverviewComponent } from 'src/app/shared/dialog-overview/dialog-overview.component';
import { StudentModel } from 'src/app/shared/base-model/student.model';

@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.scss']
})
export class StudentTableComponent implements OnInit {
  dataSource: MatTableDataSource<StudentModel>;
  displayedColumns: string[] = ['studentId', 'studentName', 'age',  'edit', 'delete'];

  constructor(private studentService: StudentService, private loaderService: LoaderService,
    private dialog: MatDialog, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadStudents();
  }

  loadStudents() {
    this.loaderService.display(true);
    this.studentService.getAllStudents()
      .subscribe((response: StudentModel[]) => {
        this.loaderService.display(false);
        this.dataSource = new MatTableDataSource(response);
      });
  }

  onEditStudent(student: StudentModel) {
    this.router.navigate(['edit', student.studentId], { relativeTo: this.route });
  }

  onDeleteStudent(student: StudentModel) {
    const dialogRef = this.dialog.open(DialogOverviewComponent, {
      width: '250px',
      data: {
        title: 'Student delete', content: 'Are you sure you want to delete the student ' + student.studentName + ' ?',
        isAlert: false
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.studentService.deleteStudent(student.studentId).subscribe(() => {
          this.loadStudents();
        });
      }
    });
  }
}
