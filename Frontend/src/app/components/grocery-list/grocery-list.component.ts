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
