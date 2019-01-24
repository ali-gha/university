import { StudentForCreateModel } from './../../../api/models/student-for-create.model';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { StudentService } from '../../../api/services/student.service';
import { MatSnackBar } from '@angular/material';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UniqueNameValidator } from '../../../api/asyncValidator/user.validator';

@Component({
  selector: 'app-student-new',
  templateUrl: './student-new.component.html',
  styleUrls: ['./student-new.component.scss']
})
export class StudentNewComponent implements OnInit {
  student: StudentForCreateModel;
  studentNewForm: FormGroup;

  constructor(private studentService: StudentService, private location: Location,
    private uniqueNameValidator: UniqueNameValidator, private snakBar: MatSnackBar) { }

  ngOnInit() {
    this.studentNewForm = new FormGroup({
      name: new FormControl(null, {
        validators: Validators.required,
        asyncValidators: this.uniqueNameValidator.validate.bind(
          { inputObject: { studentService: this.studentService } }),
        updateOn: 'blur'
      }),
      age: new FormControl(null, [Validators.required]),
    });
  }
  onSave() {
    if (!this.studentNewForm.valid) {
      this.studentNewForm.controls['name'].markAsTouched();
      this.studentNewForm.controls['age'].markAsTouched();
      return;
    }

    const value = this.studentNewForm.value;

    this.student = new StudentForCreateModel();
    this.student.name = value.name;
    this.student.age = value.age;

    this.studentService.addStudent(this.student).subscribe(() => {
      this.snakBar.open(this.student.name + ' has been added', '', {
        duration: 2000,
      });
      this.location.back();
    });
  }

  onCancel() {
    this.location.back();
  }
}
