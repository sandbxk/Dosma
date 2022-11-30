import { Component, OnInit } from '@angular/core';
import {map, Observable, startWith} from "rxjs";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-new-item',
  templateUrl: './new-item.component.html',
  styleUrls: ['./new-item.component.scss']
})
export class NewItemComponent implements OnInit {


  formControlGroup: FormGroup = new FormGroup({});

  title: string = '';
  quantity: number = 1;
  categories: string[] = ['None', 'Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  filteredCategories: Observable<string[]> = new Observable<string[]>();
  category: string = 'None';


  constructor() { }

  ngOnInit(): void {
    this.formControlGroup = new FormGroup({
      title: new FormControl(this.title, [
        Validators.required]),
      quantity: new FormControl(this.quantity, [
        Validators.required]),
      category: new FormControl()
    });
/*
    this.filteredCategories = this.formControlGroup.get('category')?.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || ''))
    );

 */
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.categories.filter(categories => categories.toLowerCase().includes(filterValue));
  }

  getTitle(): string {
    return this.formControlGroup.get('title')?.value;
  }

  getQuantity(): number {
    return this.formControlGroup.get('quantity')?.value;
  }

  addItem() {

  }

  cancel() {

  }
}
