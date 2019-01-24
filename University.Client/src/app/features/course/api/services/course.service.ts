import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/observable';
import { CourseModel } from '../models/course.model';
import { GenericService } from 'src/app/shared/services/generic.service';
import { CourseForCreateModel } from '../models/course-for-create.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable()
export class CourseService extends GenericService {
  constructor(http: HttpClient) {
    super(http);
    this.url = `${environment.apiUrl}api/course`;
    this.resourceName = 'Course';
  }
  getCourseById(id: number): Observable<CourseModel> {
    return this.getEntityById<CourseModel>(id);
  }

  getAllCourses(): Observable<CourseModel[]> {
    return this.getEntities<CourseModel>();
  }

  addCourse(course: CourseForCreateModel): Observable<CourseForCreateModel> {
    return this.addEntity<CourseForCreateModel>(course);
  }

  deleteCourse(id: number) {
    return this.deleteEntity(id);
  }

  updateCourse(id: number, course: CourseForCreateModel) {
    return this.updateEntity<CourseForCreateModel>(id, course);
  }
}
