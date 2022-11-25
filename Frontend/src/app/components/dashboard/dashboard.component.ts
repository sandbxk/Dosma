import { Component, OnInit } from '@angular/core';
import {GroceryList} from '../../interfaces/GroceryList';
import {animate, keyframes, state, style, transition, trigger} from "@angular/animations";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmationDialogComponent} from "../../dialogs/confirmation-dialog/confirmation-dialog.component";
import {CreateListDialogComponent} from "../../dialogs/create-list-dialog/create-list-dialog.component";
import {EditListDialogComponent} from "../../dialogs/edit-list-dialog/edit-list-dialog.component";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  animations: [  // This is the animation for the items, triggered upon adding and removing items
    trigger("inOutAnimation", [
      state("in", style({ opacity: 1 })),
      transition(":enter", [
        animate(
          300,
          keyframes([
            style({ opacity: 0, offset: 0 }),
            style({ opacity: 0.25, offset: 0.25 }),
            style({ opacity: 0.5, offset: 0.5 }),
            style({ opacity: 0.75, offset: 0.75 }),
            style({ opacity: 1, offset: 1 }),
          ])
        )
      ]),
      transition(":leave", [
        animate(
          300,
          keyframes([
            style({ opacity: 1, offset: 0 }),
            style({ opacity: 0.75, offset: 0.25 }),
            style({ opacity: 0.5, offset: 0.5 }),
            style({ opacity: 0.25, offset: 0.75 }),
            style({ opacity: 0, offset: 1 }),
          ])
        )
      ])
    ])
  ]
})
export class DashboardComponent implements OnInit {

  groceryLists: GroceryList[] = [];

  constructor(
    private dialogue: MatDialog
  ) { }

  ngOnInit(): void {
  
  }
  
//TODO redo colours, routing, menu styling

  newGroceryList() {
    let dialogueRef = this.dialogue.open(CreateListDialogComponent);

    dialogueRef.afterClosed().subscribe(result => {
      if (result !== undefined && result !== null) {
        let newList: GroceryList = result;
        this.groceryLists.splice(0, 0, newList);
        //HTTP add LIST
      }
    });

  }

  selectList(list: GroceryList) {
    //Routing to list
  }

  editList(list: GroceryList) {
    let dialogueRef = this.dialogue.open(EditListDialogComponent, {
      data: {
        groceryList: list
      }
    });

    dialogueRef.afterClosed().subscribe(result => {
      if (result !== null) {
        let newList: GroceryList = result;
        this.groceryLists.findIndex(x => x.id === newList.id);
        // TODO: HTTP PATCH LIST
      }
    });
  }

  deleteList(list: GroceryList) {
    let dialogueRef = this.dialogue.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Grocery List',
        message: "Are you sure you want to delete this grocery list?",
      }
    });

    dialogueRef.afterClosed().subscribe(userSaidYes => {
      if (userSaidYes) {
        this.groceryLists.splice(this.groceryLists.indexOf(list), 1);
        //TODO: HTTP DELETE LIST
      }
    });
  }

  duplicateList(list: GroceryList) {
    //HTTP DUPLICATE LIST
    let duplicateGroceryList: GroceryList = {
      id: -1,
      title: list.title,
      listItems: list.listItems,
      created: new Date(),
      modified: new Date()
    };

    this.groceryLists.splice(0,0, duplicateGroceryList);
  }
}
