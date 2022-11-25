import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-grocery-list',
  templateUrl: './grocery-list.component.html',
  styleUrls: ['./grocery-list.component.scss']
})
export class GroceryListComponent implements OnInit {

  constructor(private route: ActivatedRoute) { }

  routeId: any = {};

  ngOnInit(): void {
    this.routeId = this.route.snapshot.paramMap.get('id');
  }

}
