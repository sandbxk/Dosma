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
    private dialogue: MatDialog,
    private httpService: HttpGroceryListService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.httpService.getAllLists().then(lists => {
      this.groceryLists = lists;
    });
  }

//TODO redo colours, routing, menu styling

  newGroceryList() {
    let dialogueRef = this.dialogue.open(CreateListDialogComponent);

    dialogueRef.afterClosed().subscribe(async result => {
      if (result !== undefined && result !== null) {

        let dto = {
          title: result
        }

        const createdList = await this.httpService.createList(dto);
        this.groceryLists.splice(0, 0, createdList);
      }
    });

  }

  selectList(list: GroceryList) {
    const listData: NavigationExtras = {state: {data: list}};
    this.router.navigate([`grocery-list/${list.id}`], listData);
  }

  editList(list: GroceryList) {
    let dialogueRef = this.dialogue.open(EditListDialogComponent, {
      data: {
        groceryList: list
      }
    });

    dialogueRef.afterClosed().subscribe(async editedList => {
      if (editedList !== null) {
        const patchedList = await this.httpService.updateList(editedList);

        let index = this.groceryLists.findIndex(x => x.id === patchedList.id);
        this.groceryLists[index] = patchedList;
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
        this.httpService.deleteList(list).then(() => {
          this.groceryLists.splice(this.groceryLists.indexOf(list), 1);
        })
          .catch(err => {
          console.error(err);
        });
      }
    });
  }


  async duplicateList(list: GroceryList) {
    //HTTP DUPLICATE LIST

    const duplicateList = await this.httpService.duplicateList(list.id);
    //TODO: add to list
    this.groceryLists.splice(0, 0, list);
  }


}
