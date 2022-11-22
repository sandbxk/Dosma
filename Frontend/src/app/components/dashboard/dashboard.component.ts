import { Component, OnInit } from '@angular/core';
import {GroceryList} from '../../interfaces/GroceryList';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  groceryLists: GroceryList[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  newGroceryList() {
    this.groceryLists.push({
      id: -1,
      title: "New List", created: new Date(), modified: new Date(),
      listItems: []
    });
  }

  selectList(list: GroceryList) {

  }

  editList(list: GroceryList) {

  }

  deleteList(list: GroceryList) {

  }
}
