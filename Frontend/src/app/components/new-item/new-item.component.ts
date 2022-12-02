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


  categories: string[] = ['None', 'Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  groceryListId: number = 0;


  constructor(
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.formControlGroup = new FormGroup({
      title: new FormControl('', [
        Validators.required]),
      quantity: new FormControl(1, [
        Validators.required, Validators.min(1), Validators.max(99)]),
      category: new FormControl('None', [FormCustomValidators.autocompleteStringValidator(this.categories)])
    });


    this.dataService.currentListStageObject.subscribe(list => {
      this.groceryListId = list.id
    }).unsubscribe();
  }

  get title() {
    return this.formControlGroup.get('title');
  }

  get quantity() {
    return this.formControlGroup.get('quantity');
  }

  get category() {
    return this.formControlGroup.get('category');
  }

  addItem() {
    if (this.category?.errors) {
      this.category?.setValue('None');
    }

    if (this.formControlGroup.valid) {

      const newItem: Item = {
        id: 0,
        title: this.capitalize(this.title?.value),
        quantity: this.quantity?.value,
        groceryListId: this.groceryListId,
        status: 0,
        category: this.category?.value
      }

      this.newItemEvent.emit(newItem);
    }

  }

  //TODO:
  // scroll to
  // add new item db

  close() {
    this.cancelItemCreationEvent.emit(false);
  }

  private capitalize(str: string): string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }
}
