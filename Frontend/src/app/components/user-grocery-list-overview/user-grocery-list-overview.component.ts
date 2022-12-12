import { Component, OnInit } from '@angular/core';
import {GroceryList} from '../../interfaces/GroceryList';
import {animate, keyframes, state, style, transition, trigger} from "@angular/animations";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmationDialogComponent} from "../../dialogs/confirmation-dialog/confirmation-dialog.component";
import {CreateListDialogComponent} from "../../dialogs/create-list-dialog/create-list-dialog.component";
import {EditListDialogComponent} from "../../dialogs/edit-list-dialog/edit-list-dialog.component";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";
import {Item} from "../../interfaces/Item";
import {ActivatedRoute, NavigationExtras, Router, RouterLink, RouterModule} from "@angular/router";
import {MockLists} from "./mockLists";
import {DataService} from "../../../services/data.service";

@Component({
  selector: 'app-user-grocery-list-overview',
  templateUrl: './user-grocery-list-overview.component.html',
  styleUrls: ['./user-grocery-list-overview.component.scss'],
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
export class UserGroceryListOverviewComponent implements OnInit {

  groceryLists: GroceryList[] = [];

  constructor(
    private dialog: MatDialog,
    private httpService: HttpGroceryListService,
    private router: Router,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.httpService.getAllLists().then(lists => { //Get all the users lists
      this.groceryLists = lists;
    });



    //TODO TEMP
    this.groceryLists = MockLists;

  }

  /**
   * Opens the create list dialog. If the user creates a list, it is added to the list of groceryLists and posted to the server
   */
  newGroceryList() {
    let dialogueRef = this.dialog.open(CreateListDialogComponent); // Open the create list dialog

    dialogueRef.afterClosed().subscribe(async result => { // Subscribe to the observable returned by the dialog
      if (result !== undefined && result !== null) { // If the user created a list

        let dto = {
          title: result
        }

        const createdList = await this.httpService.createList(dto);
        this.groceryLists.splice(0, 0, createdList); // Add the created list to the list of groceryLists
      }
    }).unsubscribe(); // Unsubscribe from the observable to prevent memory leaks

  }

  /**
   * Opens the selected grocery list by navigating to the list page, which uses the grocery-list component
   * @param list
   */
  selectList(list: GroceryList) {
    this.dataService.updateListObject(list); //Update the list object in the data service so that it can be accessed by other components
    this.router.navigate([`grocery-list/${list.id}`]);
  }

  /**
   * Opens the edit list dialog. If the user edits the list name, the list is updated in the list of groceryLists and posted to the server
   * @param list
   */
  editList(list: GroceryList) {
    let dialogueRef = this.dialog.open(EditListDialogComponent, {
      data: { // Pass the list to the dialog so that it can be edited
        groceryList: list
      }
    });

    dialogueRef.afterClosed().subscribe(async editedList => {
      if (editedList !== null) {
        const patchedList = await this.httpService.updateList(editedList); // Update the edited list on the server

        let index = this.groceryLists.findIndex(x => x.id === patchedList.id); // Find the index of the list in the list of groceryLists
        this.groceryLists[index] = patchedList; // Update the list in the list of groceryLists
      }
    }).unsubscribe(); // Unsubscribe from the observable to prevent memory leaks
  }

  deleteList(list: GroceryList) {
    let dialogueRef = this.dialog.open(ConfirmationDialogComponent, { // Open the confirmation dialog
      data: { // The message to be displayed in the dialog
        title: 'Delete Grocery List',
        message: "Are you sure you want to delete this grocery list?",
      }
    });

    dialogueRef.afterClosed().subscribe(userSaidYes => {
      if (userSaidYes) {
        this.httpService.deleteList(list).then(() => { // Delete the list from the server
          this.groceryLists.splice(this.groceryLists.indexOf(list), 1); // Remove the list from the list of groceryLists
        })
          .catch(err => {
          console.error(err);
        });
      }
    }).unsubscribe(); // Unsubscribe from the observable to prevent memory leaks
  }


  async duplicateList(list: GroceryList) {
    //HTTP DUPLICATE LIST

    const duplicateList = await this.httpService.duplicateList(list.id);
    //TODO: add to list
    this.groceryLists.splice(0, 0, list);
  }


}
