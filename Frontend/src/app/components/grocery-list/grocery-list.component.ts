import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {GroceryList} from "../../interfaces/GroceryList";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss']
})
export class GroceryListComponent implements OnInit {

  constructor(private currentRoute: ActivatedRoute,
              private router: Router) { }

  groceryList: GroceryList = {
    id: 0,
    title: '',
    items: []
  };

  routeId: any = {};

  ngOnInit(): void {
    this.routeId = this.currentRoute.snapshot.paramMap.get('id');
    const nav = this.router.getCurrentNavigation();
    if (nav?.extras.state) {
      this.groceryList = nav.extras.state as GroceryList;
    }
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.groceryList.items, event.previousIndex, event.currentIndex);
  }

}
