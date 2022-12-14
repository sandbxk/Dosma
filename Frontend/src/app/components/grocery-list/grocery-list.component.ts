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
import {animate, keyframes, state, style, transition, trigger} from "@angular/animations";
import {SyncService} from "../../../services/sync.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Status} from "../../interfaces/StatusEnum";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss'],
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
    groceryListId: 0,
    index: -1
  });

  // Valid categories for items
  categories: string[] = [];
  // The ID of the list we are currently viewing
  routeId: any = {};

  // Controls the visibility of the item creation panel
  creatingItem: boolean = false;

  selectedItems: Item[] = [];
  editingItem: Item = this.placeholderItem;

  syncing: boolean = false;


  constructor(
    private currentRoute: ActivatedRoute,
    private router: Router,
    private dataService: DataService,
    private httpService: HttpGroceryListService,
    private dialog: MatDialog,
    private syncService: SyncService,
    private matSnackBar: MatSnackBar
  ) { }


  async ngOnInit(): Promise<void> {

    // Get the id from the route.
    this.routeId = this.currentRoute.snapshot.paramMap.get('id');

    // Get categories from server
    this.categories = await this.httpService.getCategories();

    // Get the grocery list from the data service, passed from the previous page
    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryList = list;
    }).unsubscribe();

    // If the id is 0, the data service did not pass a list, so we need to get it from the server
    if (this.groceryList.id === 0) {
      this.groceryList = await this.httpService.getListById(this.routeId);
    }

    this.applyAndSortIndexes();

    setInterval(this.sync, 120000); // Sync every 2 minutes
  }



  applyAndSortIndexes() {
    const localItemsStorage = localStorage.getItem(this.routeId.toString()); // Get the list from local storage
    if (localItemsStorage) { // If the list exists in local storage

      let localItems: Item[] = JSON.parse(localItemsStorage); // Parse the list

      let indexes = localItems.map(i => i.id); // Get the indexes of the item.ids in the list

      for (let i = 0; i < this.groceryList.items.length; i++) {
        let index = indexes.indexOf(this.groceryList.items[i].id); // Get the index of the item with the same id in the local storage list
        if (index !== -1) {
          this.groceryList.items[i].index = localItems[index].index; // Set the index of the item in the list to the index of the item in the local storage list
        }
      }

        this.groceryList.items.sort((a, b) => { // Sort the items by index
          console.log(a.index, b.index);
          return a.index - b.index;
        }); // Sort the items by their index
    }
  }






  /**
   * From IComponentCanDeactivate
   * Will prevent the user from navigating away from the page if there are unsaved changes
   * used for SyncService
   */
  @HostListener('window:beforeunload', ['$event'])
  canDeactivate(): boolean | Observable<boolean> {
    while (this.syncing) {
      // Wait for the sync to finish
    }
    return true;
  }

  /**
   * Called when the user drops an item in the list
   * @param event
   */
  drop(event: CdkDragDrop<Item>) {
    moveItemInArray(this.groceryList.items, event.previousIndex, event.currentIndex);
    event.item.data.index = event.currentIndex;

    //Update the index of all items in the list
    for (let i = 0; i < this.groceryList.items.length; i++) {
      this.groceryList.items[i].index = i;
    }

    localStorage.setItem(this.routeId.toString(), JSON.stringify(this.groceryList.items));
  }

  /**
   * Syncs the local list with the server
   * Starts with syncing down and notifying the user of any changes present on the server,
   * the user can discard the changes or merge them with the local list
   * Then the local list is synced up to the server
   */
  async sync() {
    this.syncing = true;

    try {

    const updatedList = await this.syncService.syncDown()
    if (updatedList.id !== 0 && updatedList !== this.groceryList) { //Must not be the placeholder list, and should not be identical to the current list

      this.dialog.open(ConfirmationDialogComponent, {
        data: {
          title: "New changes detected",
          message: "Would you like to update your list with the new changes? " +
            "This will overwrite any recent changes you have made.",
        }
      }).afterClosed().subscribe((result: boolean) => {
        if (result) {
          this.groceryList = updatedList;
        }
      }).unsubscribe();
    }

    this.syncService.syncUp(this.groceryList).then(async () => {
    })
      .catch(reason => this.matSnackBar
        .open(reason, "Dismiss", {duration: 5000}))
          .finally(() => this.syncing = false);


    } catch (error) {
      this.syncing = false;
      this.matSnackBar.open('ERROR: Could not synchronize', "Dismiss", {duration: 5000});
    }
  }


 /**
  * Navigates back to the dashboard with all the user's grocery lists
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

        this.httpService.deleteList(this.groceryList.id).then((result) => {
          if (result)
            this.navigateBack();
          else
            this.matSnackBar.open('ERROR: Could not delete list', "Dismiss", {duration: 5000});
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
      this.selectedItems.splice(this.selectedItems.indexOf(item), 1); // Remove the item from the array

    else
      this.selectedItems.push(item); // Add the item to the array
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


  deleteItems() {
    const itemsToDelete = this.selectedItems;

    let deleteMessage = "";

    if (itemsToDelete.length === 1)
      deleteMessage = "Are you sure you want to delete this item?";
    else
      deleteMessage = `Are you sure you want to delete these ${itemsToDelete.length} items?`;

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Delete Items',
        message: deleteMessage
      }
    });

    dialogRef.afterClosed().subscribe(userSaidYes => {
      if (userSaidYes) {
        const setOfItemsToDelete = new Set(itemsToDelete);
        const newGroceryListItems = this.groceryList.items.filter((item) => {
          return !setOfItemsToDelete.has(item);
        });
        this.groceryList.items = newGroceryListItems;
      }
    });
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
    if (cancel) {
      this.editingItem = this.placeholderItem;
    }
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

  get statusEnum(){
    return Status;
  }

  duplicateSelectedItem() {
    let selectedItem = this.selectedItems[0]; // Get the selected item

    //DTO
    const duplicateDTO = {
      title: selectedItem.title,
      quantity: selectedItem.quantity,
      status: selectedItem.status,
      groceryListId: selectedItem.groceryListId,
      category: selectedItem.category
    }

    //Post the duplicated item
    this.httpService.duplicateItem(duplicateDTO).then((item) => {
      let duplicatedItem: Item = { // add missing properties
        id: item.id,
        groceryListId: item.groceryListId,
        index: this.selectedItems.length,
        quantity: item.quantity,
        status: item.status,
        title: item.title,
        category: item.category
      }

      this.groceryList.items.push(duplicatedItem);
    });

  }
}
