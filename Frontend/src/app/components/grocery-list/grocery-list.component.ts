import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";
import {DataService} from "../../../services/data.service";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";
import {IComponentCanDeactivate} from "../../../services/PendingChanges.guard";
import {Observable, timeout} from "rxjs";
import {Item} from "../../interfaces/Item";
import {ConfirmationDialogComponent} from "../../dialogs/confirmation-dialog/confirmation-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {EditListDialogComponent} from "../../dialogs/edit-list-dialog/edit-list-dialog.component";
import {NewItemComponent} from "../new-item/new-item.component";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss']
})
export class GroceryListComponent implements OnInit, IComponentCanDeactivate {

  // Grocery list variable for the list the user currently has open
  groceryList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  // Valid categories for items
  categories: string[] = ['Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  // The ID of the list we are currently viewing
  routeId: any = {};

  // Controls the visibility of the item creation panel
  creatingItem: boolean = false;

  selectedItem: Item = {} as Item;

  constructor(
    private currentRoute: ActivatedRoute,
    private router: Router,
    private dataService: DataService,
    private httpService: HttpGroceryListService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    // Get the id from the route.
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
  // 2. Edit an item in the list -> ItemComponent
  // 3. Delete an item from the list
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


  /**
   * From IComponentCanDeactivate
   * Will prevent the user from navigating away from the page if there are unsaved changes
   * To be used for SyncService
   */
  @HostListener('window:beforeunload', ['$event'])
  canDeactivate(): boolean | Observable<boolean> {
    // insert logic to check if there are pending changes here;
    // returning true will navigate without confirmation
    // returning false will show a confirm dialog before navigating away
    return true;
  }

  /**
   * Called when the user drops an item in the list
   * @param event
   */
  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.groceryList.items, event.previousIndex, event.currentIndex);
  }

 /**
  * Navigates back to the dashboard with all the users grocery lists
  */
  navigateBack() {
    this.router.navigate(['/dashboard']);
  }

  /**
   * Deletes the current list
   * Will display a dialog to confirm the deletion.
   * If the user confirms, the list will be deleted from the server and the user will be navigated back to the dashboard
   */
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

  /**
   * sets the item to the selected item, which will apply the selected class to the item
   * @param item
   */
  selectItem(item: Item) {
    if (this.selectedItem === item)
      this.selectedItem = {} as Item;

    else
      this.selectedItem = item;
  }

  /**
   * Used on the item creation panel to create a new item
   * @param $event The item to be added to the list, emitted from the new-item component
   */
  addItem($event: Item) {
    this.groceryList.items.push($event);
    const scrollToItem = () => this.scrollToItemCreationPanel()
    setTimeout(scrollToItem, 250); // Timeout is required as an additional element comes into view when the panel is shown, changing the scroll position
  }

  /**
   * Will toggle the visibility of the item creation panel
   * If the panel is to be made visible, the panel will be scrolled into view
   * @param boolean
   */
  showNewItemPanel(showing: boolean) {
    this.creatingItem = showing;

    if (showing) {
      const scrollToItem = () => this.scrollToItemCreationPanel()
      setTimeout(scrollToItem, 250); // Timeout is required as an additional element comes into view when the panel is shown, changing the scroll position
    }
  }

  scrollToItemCreationPanel() {
     const element: HTMLElement = document.getElementById("item-creation-panel")!
      element.scrollIntoView({
        behavior: "smooth",
        block: "start",
        inline: "nearest"
      });

     return element;
  }


  editItem(item: Item) {

  }

  duplicateItem(item: Item) {

  }

  deleteItem(item: Item) {

  }

  /**
   * Will open a dialog to edit the list title
   */
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
