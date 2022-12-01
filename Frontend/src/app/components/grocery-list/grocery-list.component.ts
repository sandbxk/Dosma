import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";
import {DataService} from "../../../services/data.service";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";
import {ComponentCanDeactivate} from "../../../services/PendingChanges.guard";
import {Observable} from "rxjs";
import {Item} from "../../interfaces/Item";
import {ConfirmationDialogComponent} from "../../dialogs/confirmation-dialog/confirmation-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {EditListDialogComponent} from "../../dialogs/edit-list-dialog/edit-list-dialog.component";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss']
})
export class GroceryListComponent implements OnInit, ComponentCanDeactivate {

  groceryList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  categories: string[] = ['Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  routeId: any = {};

  creatingItem: boolean = false;

  constructor(
    private currentRoute: ActivatedRoute,
    private router: Router,
    private dataService: DataService,
    private httpService: HttpGroceryListService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    // Get the id from the route
    this.routeId = this.currentRoute.snapshot.paramMap.get('id');

    // Get the grocery list from the data service, passed from the previous page
    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryList = list;
    }).unsubscribe();

    // If the id is 0, the data service did not pass a list, so we need to get it from the server
    if (this.groceryList.id === 0) {
      this.httpService.getListById(this.routeId).then((list: GroceryList) => {
        this.groceryList = list;
      });
    }
  }

  //TODO:
  // 1. Add a new item to the list -> ItemActionsComponent
  // // Limit quantity to 2 digits
  // 2. Edit an item in the list -> ItemComponent
  // 3. Delete an item from the list
  // 4. Duplicate an item in the list
  //      Change mat menu for items to selected item options
  //   ListMenu Options
  //      Delete all items from the list?
  //       Mark all items as purchased
  //
  // https://m2.material.io/components/buttons-floating-action-button
  // Floating action button (as toggle for panel?)
  //
  //

  //TODO: Item status
  // 4. Mark an item as purchased -> ItemStatusComponent
  // 5. Mark an item as skipped -> ItemStatusComponent
  // 5. Mark all items as purchased -> ItemStatusComponent
  //          ListMenu Options
  // On check, opacity 0.7


  //TODO: Sync
  // 7. Sync on timer -> SyncService?
  // 8. Sync on button press -> SyncService?

  //TODO Layout
  // Create menu for item options



  @HostListener('window:beforeunload', ['$event'])
  canDeactivate(): boolean | Observable<boolean> {
    // insert logic to check if there are pending changes here;
    // returning true will navigate without confirmation
    // returning false will show a confirm dialog before navigating away
    return false;
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.groceryList.items, event.previousIndex, event.currentIndex);
  }


  navigateBack() {
    this.router.navigate(['/dashboard']);
  }

  deleteList() {
    let dialogueRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Grocery List',
        message: "Are you sure you want to delete this grocery list?",
      }
    });

    dialogueRef.afterClosed().subscribe(userSaidYes => {
      if (userSaidYes) {
        this.httpService.deleteList(this.groceryList).then(() => {
          this.navigateBack();
        })
          .catch(err => {
            console.error(err);
          });
      }
    });
  }

  addItem($event: Item) {
    this.groceryList.items.push($event);
  }

  hideNewItemPanel(boolean: boolean) {
    this.creatingItem = !boolean;
  }

  editItem(item: Item) {

  }

  duplicateItem(item: Item) {

  }

  deleteItem(item: Item) {

  }

  editListName() {
      let dialogueRef = this.dialog.open(EditListDialogComponent, {
        data: {
          groceryList: this.groceryList
        }
      });

      dialogueRef.afterClosed().subscribe(async editedList => {
        if (editedList !== null) {
          this.groceryList.title = editedList.title;
        }
      }).unsubscribe();
  }


}
