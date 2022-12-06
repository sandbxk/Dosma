import {Component, HostListener, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";
import {DataService} from "../../../services/data.service";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";
import {IComponentCanDeactivate} from "../../../services/PendingChanges.guard";
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
export class GroceryListComponent implements OnInit, IComponentCanDeactivate {

  // Grocery list variable for the list the user currently has open
  groceryList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  // Invalid item used to reset the editing item. Object.Freeze is used to prevent the object from being modified, making it immutable
  placeholderItem: Item = Object.freeze({
    id: 0,
    title: "Placeholder",
    quantity: 0,
    category: "None",
    status: 0,
    groceryListId: 0
  });

  // Valid categories for items
  categories: string[] = ['Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  // The ID of the list we are currently viewing
  routeId: any = {};

  // Controls the visibility of the item creation panel
  creatingItem: boolean = false;

  selectedItems: Item[] = [];
  editingItem: Item = this.placeholderItem;


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
  // 1. Delete an item from the list
  // 2. ListMenu Options
  //    - Delete all items from the list?
  //    - Mark all items as purchased

  //TODO: Item status
  // 4. Mark an item as purchased -> ItemStatusComponent
  // 5. Mark an item as skipped -> ItemStatusComponent
  // 5. Mark all items as purchased -> ListMenu Options
  // On check, opacity 0.7 and strikethrough
  // On skipped, color warn-light, opacity 0.7 and strikethrough


  //TODO: Sync
  // 7. Sync on timer -> SyncService?
  // 8. Sync on button press -> SyncService?


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
  drop(event: CdkDragDrop<Item>) {
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
    if (this.selectedItems.includes(item))
      this.selectedItems.splice(this.selectedItems.indexOf(item), 1);

    else
      this.selectedItems.push(item);
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
   * @param showing
   */
  showNewItemPanel(showing: boolean) {
    this.creatingItem = showing;

    if (showing) {
      this.cancelEditItem(true) // Cancel any item editing and close the editing panel

      const scrollToItem = () => this.scrollToItemCreationPanel()
      setTimeout(scrollToItem, 250); // Timeout is required as an additional element comes into view when the panel is shown, changing the scroll position
    }

  }

  /**
   * Scrolls the item creation panel into view
   */
  scrollToItemCreationPanel() {
     const element: HTMLElement = document.getElementById("item-creation-panel")!
      element.scrollIntoView({
        behavior: "smooth",
        block: "start",
        inline: "nearest"
      });
  }


  /**
   * Replaces the original item with the edited item emitted from the edit-item component
   * @param $event
   */
  editItem($event: Item) {
    let index = this.groceryList.items.indexOf(this.editingItem);
    this.groceryList.items[index] = $event;
    this.editingItem = this.placeholderItem;
  }


  deleteItem(item: Item) {

  }

  /**
   * Clears the selected items array and closes the editing panel
   */
  clearSelectedItems() {
    this.selectedItems = [];
    this.cancelEditItem(true);
  }

  /**
   * Cancels the editing of an item by setting it to the invalid placeholder item
   * @param cancel
   */
  cancelEditItem(cancel: boolean) {
    if (cancel)
      this.editingItem = this.placeholderItem;
  }

  /**
   * Will open a dialog to edit the list title
   */
  editListName() {
    // Open a dialog to edit the list name and pass the current grocery list
      let dialogueRef = this.dialog.open(EditListDialogComponent, {
        data: {
          groceryList: this.groceryList
        }
      });

      //Set the new list name after the dialog closes
      dialogueRef.afterClosed().subscribe(async editedList => {
        if (editedList !== null) {
          this.groceryList.title = editedList.title;
        }
      }).unsubscribe(); // Unsubscribe to prevent memory leaks
  }

  /**
   * Automatically disables drag and drop if the editing dialog or creation panel is open
   * Does so through the cdkDropListDisabled directive
   */
  dragAndDropIsDisabled() {
    if (this.editingItem.id !== 0) //If an item is being edited, disable drag and drop
      return true;
    if (this.creatingItem) //If the item creation panel is open, disable drag and drop
      return true;

    return false;
  }

  /**
   * Will open a dialog to edit the selected item
   */
  startEditingItem() {
    if (this.selectedItems.length === 1) {
      this.editingItem = this.selectedItems[0];  // Set the item to be edited to the selected item
      this.showNewItemPanel(false);  // Close the new item panel if it is open
    }
  }
}
