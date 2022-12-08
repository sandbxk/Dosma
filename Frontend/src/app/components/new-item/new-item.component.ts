import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
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

  // Event types for the parent component
  @Output() newItemEvent = new EventEmitter<Item>();
  @Output() closeItemCreationEvent = new EventEmitter<boolean>();

  formControlGroup: FormGroup = new FormGroup({});

  // List of categories for the autocomplete
  categories: string[] = ['None', 'Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER
  // ID for the grocery list.
  @Input() groceryListId: number = 0;

  @Input() groceryListSize: number = 0;


  constructor() { }

  ngOnInit(): void {
    // Initialize the formGroup and its controls
    this.formControlGroup = new FormGroup({
      title: new FormControl('', [
        Validators.required]), //required
      quantity: new FormControl(1, [
        Validators.required, Validators.min(1), Validators.max(99)]), //Range 1-99, required
      category: new FormControl('None',
        [FormCustomValidators.autocompleteStringValidator(this.categories)]) //Use custom validator to check if the category is in the list
    });

  }

  /**
   * Gets the title value from the title FormControl, present in the formControlGroup.
   * The title will be validated in the form control.
   */
  get title() {
    return this.formControlGroup.get('title');
  }

  /**
   * Gets the quantity value from the quantity FormControl, present in the formControlGroup.
   * The quantity will be validated in the form control.
   */
  get quantity() {
    return this.formControlGroup.get('quantity');
  }

  /**
   * Gets the category value from the category FormControl, present in the formControlGroup.
   * The category will be validated in the form control.
   */
  get category() {
    return this.formControlGroup.get('category');
  }

  /**
   * Creates a new item and emits it to the parent component.
   */
  addItem() {
    if (this.category?.errors) { //If the category is not valid, set it to 'None'
      this.category?.setValue('None');
    }

    if (this.formControlGroup.valid) {  //If the form is valid, create the item and emit it to the parent component

      const newItem: Item = {
        id: 0,
        title: this.capitalize(this.title?.value),
        quantity: this.quantity?.value,
        groceryListId: this.groceryListId,
        status: 0,
        category: this.category?.value,
        index: this.groceryListSize
      }

      this.newItemEvent.emit(newItem);
    }

  }

  //TODO:
  // add new item db

  /**
   * Emits an event to the parent component to close the item creation panel
   */
  close() {
    this.closeItemCreationEvent.emit(true);
  }

  /**
   * Capitalizes the first letter of a string
   * @param str
   * @private
   */
  private capitalize(str: string): string {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }
}
