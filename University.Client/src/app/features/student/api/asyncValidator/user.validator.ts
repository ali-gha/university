import { FormControl } from '@angular/forms';
import { Injectable, Input } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { StudentService } from '../services/student.service';


@Injectable({
    providedIn: 'root',
})
export class UniqueNameValidator {
    @Input() inputObject: { oldValue: string , studentService: StudentService };

    validate(c: FormControl): Promise<any> | Observable<any> {
        if (this.inputObject.oldValue !== c.value) {
            return this.inputObject.studentService.checkStudentName(c.value)
            .pipe(
                map(d => {
                    return d ? {
                        isExistUserName: d
                    } : null;
                }),
            );
        } else {
            return of(null);
        }
    }
}

