import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from "@angular/material/dialog";
import {faker} from "@faker-js/faker";
import {GroceryList} from "../../interfaces/GroceryList";

@Component({
  selector: 'app-create-list-dialog',
  templateUrl: './create-list-dialog.component.html',
  styleUrls: ['./create-list-dialog.component.scss']
})
export class CreateListDialogComponent implements OnInit {

  title: string = "";

  constructor(
    public dialogRef: MatDialogRef<CreateListDialogComponent>,
  )
  { }

  ngOnInit(): void {
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSave() {
    this.dialogRef.close(this.createListObj())
  }

  private createListObj() {
    let postTitle = this.title.trim();

    if (postTitle === ''){
      postTitle = faker.word.noun() //Use emoji??
    }

    const capitalizedTitle = postTitle.charAt(0).toUpperCase() + postTitle.slice(1);

    let groceryList = {
      id: -1,
      title: capitalizedTitle,
      listItems: [],
      created: new Date(),
      modified: new Date()
    }

    return groceryList as GroceryList;
  }


}
