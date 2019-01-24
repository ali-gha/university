import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent {
  constructor(private router: Router, private route: ActivatedRoute) { }

  onAddCourse() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }
}



