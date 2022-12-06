import { Component, Input } from '@angular/core';
import { MatCheckbox } from '@angular/material/checkbox';
import {Status} from "../../interfaces/StatusEnum";




/**
 * A component that displays a checkbox with three states - 'done', 'skipped', and unchecked.
 */
@Component({
  selector: 'app-tri-state-checkbox',
  templateUrl: './tri-state-checkbox.component.html',
  styles: [`
    @import 'tri-state-checkbox.component.scss';
  `]
})
export class TriStateCheckboxComponent {

  constructor() { }

  ngOnInit(): void {
  }

  /**
   * The state of the checkbox.
   */
  @Input() state: Status = Status.Unchecked;

  /**
   * Handles the change event and updates the state of the checkbox.
   * @param newState The new state of the checkbox.
   */
  onChange(newState: Status) {
    this.state = newState;
  }

  public get status() {
    return Status;
  }


}
