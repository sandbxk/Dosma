import {Component, HostListener, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";
import {DataService} from "../../../services/data.service";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";
import {ComponentCanDeactivate} from "../../../services/PendingChanges.guard";
import {Observable} from "rxjs";

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

  routeId: any = {};

  constructor(
    private currentRoute: ActivatedRoute,
    private router: Router,
    private dataService: DataService,
    private httpService: HttpGroceryListService
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
  // 1. Add a new item to the list -> ItemCreatorComponent
  // 2. Edit an item in the list -> ItemEditorComponent
  // 3. Delete an item from the list and the list itself
  // 4. Mark an item as purchased -> ItemStatusComponent
  // 5. Mark an item as skipped -> ItemStatusComponent
  // 5. Mark all items as purchased -> ItemStatusComponent
  // 6. Delete all items from the list?
  // 7. Sync on timer -> SyncService?
  // 8. Sync on button press -> SyncService?

  //TODO Layout
  // Change back button location
  // truncate long titles
  // truncate long item titles
  // limit quantity to 2 digits
  // Make list title static, and have the list be scrollable
  // Create menu for list options
  // Create menu for item options
  // Align item.title and item.quantity



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
}
