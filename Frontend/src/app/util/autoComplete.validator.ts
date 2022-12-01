import {AbstractControl, ValidatorFn, Validators} from "@angular/forms";
/*
SELECTBOX_VALUE: [
  null,
  Validators.compose([
    Validators.required,
    FormCustomValidators.valueSelected(this.myArray),
  ]),
];
 */
export class FormCustomValidators {
  static valueSelected(myArray: any[]): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      let selectboxValue = c.value;
      let pickedOrNot = myArray.filter(
        (alias) => alias.name === selectboxValue
      );

      if (pickedOrNot.length > 0) {
        // everything's fine. return no error. therefore it's null.
        return null;
      } else {
        //there's no matching selectboxvalue selected. so return match error.
        return { match: true };
      }
    };
  }
}
