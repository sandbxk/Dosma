import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {map, Observable, startWith} from "rxjs";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Item} from "../../interfaces/Item";
import {DataService} from "../../../services/data.service";

@Component({
  selector: 'app-new-item',
  templateUrl: './new-item.component.html',
  styleUrls: ['./new-item.component.scss']
})
export class NewItemComponent implements OnInit {

  @Output() newItemEvent = new EventEmitter<Item>();
  @Output() creatingItemEvent = new EventEmitter<boolean>();

  formControlGroup: FormGroup = new FormGroup({});

  title: string = '';
  quantity: number = 1;
  categories: string[] = ['None', 'Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  filteredCategories: Observable<string[]> = new Observable<string[]>();
  category: string = 'None';
  groceryListId: number = 0;


  constructor(
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.formControlGroup = new FormGroup({
      title: new FormControl(this.title, [
        Validators.required]),
      quantity: new FormControl(this.quantity, [
        Validators.required]),
      category: new FormControl()
    });

    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryListId = list.id
    }).unsubscribe();
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
    const newItem: Item = {
      id: 0,
      title: this.getTitle(),
      quantity: this.getQuantity(),
      groceryListId: this.groceryListId,
      status: 0,
      category: this.category
    }

    this.newItemEvent.emit(newItem);
    this.creatingItemEvent.emit(false);
  }

  cancel() {
    this.creatingItemEvent.emit(false);
  }

  close() {

  }
}
