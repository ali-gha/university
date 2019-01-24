import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { DialogDataModel } from '../base-model/dialog-data.model';

@Component({
  selector: 'app-dialog-overview',
  templateUrl: './dialog-overview.component.html',
  styleUrls: ['./dialog-overview.component.scss']
})
export class DialogOverviewComponent {

  constructor(private dialogref: MatDialogRef<DialogOverviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel) { }

  onNoClick(): void {
    this.dialogref.close(false);
  }
}
