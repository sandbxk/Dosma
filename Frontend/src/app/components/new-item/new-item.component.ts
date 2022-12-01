import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {map, Observable, startWith} from "rxjs";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Item} from "../../interfaces/Item";
import {DataService} from "../../../services/data.service";
import {FormCustomValidators} from "../../util/autoComplete.validator";

@Component({
  selector: 'app-new-item',
  templateUrl: './new-item.component.html',
  styleUrls: ['./new-item.component.scss']
})
export class NewItemComponent implements OnInit {

  @Output() newItemEvent = new EventEmitter<Item>();
  @Output() cancelItemCreationEvent = new EventEmitter<boolean>();

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
      category: new FormControl(this.category, {validators: [Validators.required, FormCustomValidators.valueSelected(this.categories)]})
    });


    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryListId = list.id
    }).unsubscribe();
  }

  getTitle(): string {
    return this.formControlGroup.get('title')?.value;
  }

  getQuantity(): number {
    return this.formControlGroup.get('quantity')?.value;
  }

  getCategory(): string {
    return this.formControlGroup.get('category')?.value;
  }

  addItem() {
    if (this.formControlGroup.valid) {
      const newItem: Item = {
        id: 0,
        title: this.capitalize(this.getTitle()),
        quantity: this.getQuantity(),
        groceryListId: this.groceryListId,
        status: 0,
        category: this.getCategory()
      }

      this.newItemEvent.emit(newItem);
    }

  }

  //TODO:
  // scroll to
  // add new item db
  // finalize validation
      // force to choose input from autocomplete

  close() {
    this.cancelItemCreationEvent.emit(false);
  }

  private capitalize(str: string): string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }
}
