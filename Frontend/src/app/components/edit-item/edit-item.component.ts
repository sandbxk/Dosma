import {Component, Input, OnInit, Output, EventEmitter} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Item} from "../../interfaces/Item";
import {FormCustomValidators} from "../../util/autoComplete.validator";


@Component({
  selector: 'app-edit-item',
  templateUrl: './edit-item.component.html',
  styleUrls: ['./edit-item.component.scss']
})
/**
 * Component for editing an item in a grocery list.
 * Shares most of its code with the new item component, but is made separate for due to separations of concern and extendability.
 */
export class EditItemComponent implements OnInit {

  formControlGroup: FormGroup = new FormGroup({});

  // List of categories for the autocomplete
  categories: string[] = ['None', 'Fruits', 'Vegetables', 'Meat', 'Dairy', 'Bakery', 'Beverages', 'Other']; //TODO FETCH CATEGORIES FROM SERVER

  @Input() listItem: Item = {} as Item;

  @Output() editItemEvent = new EventEmitter<Item>();
  @Output() cancelEditItemEvent = new EventEmitter<boolean>();




  constructor() {

  }

  ngOnInit(): void {
    // Initialize the formGroup and its controls
    this.formControlGroup = new FormGroup({
      title: new FormControl(this.listItem.title, [
        Validators.required]), //required
      quantity: new FormControl(this.listItem.quantity, [
        Validators.required, Validators.min(1), Validators.max(99)]), //Range 1-99, required
      category: new FormControl(this.listItem.category,
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
  editItem() {
    if (this.formControlGroup.valid) {  //If the form is valid, create the item and emit it to the parent component

      const editedItem: Item = {
        id: this.listItem.id,
        title: this.capitalize(this.title?.value),
        quantity: this.quantity?.value,
        groceryListId: this.listItem.groceryListId,
        status: this.listItem.status,
        category: this.category?.value
      }

      this.editItemEvent.emit(editedItem);
    }

  }

  //TODO:
  // add new item db

  /**
   * Emits an event to the parent component to close the item creation panel
   */
  cancel() {
    this.cancelEditItemEvent.emit(true);
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
