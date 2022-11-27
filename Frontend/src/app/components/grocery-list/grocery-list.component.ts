import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";
import {DataService} from "../../../services/data.service";
import {HttpGroceryListService} from "../../../services/httpGroceryList.service";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss']
})
export class GroceryListComponent implements OnInit {

  constructor(private currentRoute: ActivatedRoute,
              private router: Router,
              private dataService: DataService,
              private httpService: HttpGroceryListService
  ) { }

  groceryList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  routeId: any = {};

  ngOnInit(): void {
    this.routeId = this.currentRoute.snapshot.paramMap.get('id');
    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryList = list;
    }).unsubscribe();

    if (this.groceryList.id === 0) {
      this.httpService.getListById(this.routeId).then((list: GroceryList) => {
        this.groceryList = list;
      });
    }

  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.groceryList.items, event.previousIndex, event.currentIndex);
  }

}
