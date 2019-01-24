import { StudentForUpdateModel } from './../models/student-for-update.model';
import { StudentForCreateModel } from './../models/student-for-create.model';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/observable';

import { GenericService } from 'src/app/shared/services/generic.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/internal/operators/map';
import { StudentModel } from 'src/app/shared/base-model/student.model';

@Injectable()
export class StudentService extends GenericService  {
    constructor(http: HttpClient) {
      super(http);
      this.url = `${environment.apiUrl}api/student`;
      this.resourceName = 'Student';
    }
    getStudentById(id: number): Observable<StudentModel> {
        return this.getEntityById<StudentModel>(id);
    }

    checkStudentName(name: string): Observable<boolean> {
      return this.httpClient.get(this.url + `/checkStudentName/${name}`)
          .pipe(
              map((res: boolean) => {
                return res;
              })
          );
  }

    getAllStudents(): Observable<StudentModel[]> {
        return this.getEntities<StudentModel>();
    }

    addStudent(studentforCreate: StudentForCreateModel): Observable<StudentForCreateModel> {
        return this.addEntity<StudentForCreateModel>(studentforCreate);
    }

    deleteStudent(id: number) {
        return this.deleteEntity(id);
    }

    updateStudent(id: number, studentforUpdate: StudentForUpdateModel) {
        return this.updateEntity<StudentForUpdateModel>(id, studentforUpdate);
    }
}
