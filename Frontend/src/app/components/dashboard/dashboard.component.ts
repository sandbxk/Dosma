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
    this.groceryLists.push({
      id: 1,
      title: 'List 1 with a very very very very very long',
      listItems: [ {id: 1, title: 'Item 1', quantity: 1, status: 0, category: 'Category 1'},
        {id: 2, title: 'Item 2 with a very very very long na', quantity: 1, status: 0, category: 'Category 1'},
        {id: 3, title: 'Item 3', quantity: 1, status: 0, category: 'Category 1'},
        {id: 4, title: 'Item 4', quantity: 1, status: 0, category: 'Category 1'},
        {id: 5, title: 'Item 5', quantity: 1, status: 0, category: 'Category 1'},
        {id: 6, title: 'Item 6', quantity: 1, status: 0, category: 'Category 1'},
        {id: 7, title: 'Item 7', quantity: 1, status: 0, category: 'Category 1'},
        {id: 8, title: 'Item 8', quantity: 1, status: 0, category: 'Category 1'},
        {id: 9, title: 'Item 9', quantity: 1, status: 0, category: 'Category 1'},
        {id: 10, title: 'Item 10', quantity: 1, status: 0, category: 'Category 1'},
        {id: 11, title: 'Item 11', quantity: 1, status: 0, category: 'Category 1'},
        {id: 12, title: 'Item 12', quantity: 1, status: 0, category: 'Category 1'},
        {id: 13, title: 'Item 13', quantity: 1, status: 0, category: 'Category 1'},
        {id: 14, title: 'Item 14', quantity: 1, status: 0, category: 'Category 1'},
        {id: 15, title: 'Item 15', quantity: 1, status: 0, category: 'Category 1'},
        {id: 16, title: 'Item 16', quantity: 1, status: 0, category: 'Category 1'},
        {id: 17, title: 'Item 17', quantity: 1, status: 0, category: 'Category 1'},
        {id: 18, title: 'Item 18', quantity: 1, status: 0, category: 'Category 1'},
        {id: 19, title: 'Item 19', quantity: 1, status: 0, category: 'Category 1'},
        {id: 20, title: 'Item 20', quantity: 1, status: 0, category: 'Category 1'},
        {id: 21, title: 'Item 21', quantity: 1, status: 0, category: 'Category 1'},
        {id: 22, title: 'Item 22', quantity: 1, status: 0, category: 'Category 1'},
        {id: 23, title: 'Item 23', quantity: 1, status: 0, category: 'Category 1'},
        {id: 24, title: 'Item 24', quantity: 1, status: 0, category: 'Category 1'},
        {id: 25, title: 'Item 25', quantity: 1, status: 0, category: 'Category 1'},
        {id: 26, title: 'Item 26', quantity: 1, status: 0, category: 'Category 1'},
        {id: 27, title: 'Item 27', quantity: 1, status: 0, category: 'Category 1'},
        {id: 28, title: 'Item 28', quantity: 1, status: 0, category: 'Category 1'},
        {id: 29, title: 'Item 29', quantity: 1, status: 0, category: 'Category 1'},
        {id: 30, title: 'Item 30', quantity: 1, status: 0, category: 'Category 1'},
        {id: 31, title: 'Item 31', quantity: 1, status: 0, category: 'Category 1'},
      ],
      created: new Date(),
      modified: new Date()
    });
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
        //HTTP PATCH LIST
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
        //HTTP DELETE LIST
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
