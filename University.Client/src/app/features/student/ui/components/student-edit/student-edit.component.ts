import { StudentForUpdateModel } from './../../../api/models/student-for-update.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { StudentService } from '../../../api/services/student.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Location } from '@angular/common';
import { UniqueNameValidator } from '../../../api/asyncValidator/user.validator';

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.scss']
})
export class StudentEditComponent implements OnInit {
  student: StudentForUpdateModel;
  id: number;
  studentEditForm: FormGroup;

  constructor(private route: ActivatedRoute, private studentService: StudentService,
    private uniqueNameValidator: UniqueNameValidator, private location: Location, private snakBar: MatSnackBar) { }

  ngOnInit() {
    this.studentEditForm = new FormGroup({
      name: new FormControl(null,  {
        validators: Validators.required,
        updateOn: 'blur'
      }),
      age: new FormControl(null, [Validators.required]),
    });

    this.id = this.route.params['value'].id;
    this.studentService.getStudentById(this.id)
      .subscribe((response) => {
        this.studentEditForm.setValue({
          name: response.studentName,
          age: response.age,
        });
        this.studentEditForm.controls['name'].setAsyncValidators(
           this.uniqueNameValidator.validate.bind(
            { inputObject: { oldValue : this.studentEditForm.controls['name'].value, studentService: this.studentService } })
        );
      });
  }

  onSave() {
    if (!this.studentEditForm.valid) {
      this.studentEditForm.controls['name'].markAsTouched();
      this.studentEditForm.controls['age'].markAsTouched();
      return;
    }

    const value = this.studentEditForm.value;
    this.student = new StudentForUpdateModel();
    this.student.name = value.name;
    this.student.age = value.age;

    this.studentService.updateStudent(this.id, this.student).subscribe(() => {
      this.snakBar.open(this.student.name + ' has been edited', '', {
        duration: 2000,
      });
      this.location.back();
    });
  }

  onCancel() {
    this.location.back();
  }
}
