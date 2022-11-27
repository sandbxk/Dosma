import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {GroceryList} from "../../interfaces/GroceryList";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-edit-list-dialog',
  templateUrl: './edit-list-dialog.component.html',
  styleUrls: ['./edit-list-dialog.component.scss']
})
export class EditListDialogComponent implements OnInit {

  groceryList: GroceryList = {
    id: -1,
    title: "",
    items: [],
    //TBA created: new Date(),
    //TBA modified: new Date()
  };

  form: any = {};


  constructor(
    public dialogRef: MatDialogRef<EditListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.groceryList = data.groceryList;
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      title: new FormControl(this.groceryList.title, [
        Validators.required,
      ])
    });
  }

  get title() {
    return this.form.get('title');
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSave() {
    if (this.title?.valid) {
      this.groceryList.title = this.title.value;
      this.dialogRef.close(this.groceryList)
    }
  }
}
