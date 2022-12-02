import {AbstractControl, ValidatorFn, Validators} from "@angular/forms";

export class FormCustomValidators {
  static autocompleteStringValidator(validOptions: Array<string>): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (validOptions.indexOf(control.value) !== -1) {
        return null  // valid option selected
      }
      return {'invalidAutocompleteString': {value: control.value}}
    }
  }


  static autocompleteObjectValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (typeof control.value === 'string') {
        return {'invalidAutocompleteObject': {value: control.value}}
      }
      return null  // valid option selected
    }
  }
}


