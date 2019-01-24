import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent {

  constructor(private router: Router, private route: ActivatedRoute) { }

  onAddStudent() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }
}
