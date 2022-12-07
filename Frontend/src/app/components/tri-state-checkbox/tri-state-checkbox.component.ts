import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Status} from "../../interfaces/StatusEnum";


/**
 * A component that displays a checkbox with three states - 'done', 'skipped', and unchecked.
 */
@Component({
  selector: 'app-tri-state-checkbox',
  templateUrl: './tri-state-checkbox.component.html',
  styleUrls: ['tri-state-checkbox.component.scss']

})
export class TriStateCheckboxComponent {

  constructor() { }

  ngOnInit(): void {
  }

  @Output() statusChangedEvent = new EventEmitter<Status>();

  /**
   * The state of the checkbox.
   */
  @Input() status: Status = Status.Unchecked;

  /**
   * Handles the change event and updates the state of the checkbox.
   * @param newState The new state of the checkbox.
   */
  onChange() {

    switch (this.status) {
      case Status.Unchecked:
        this.status = Status.Done;
        break;
      case Status.Done:
        this.status = Status.Skipped;
        break;
      case Status.Skipped:
        this.status = Status.Unchecked;
        break;
    }
    console.log(this.status.toString());
    this.statusChangedEvent.emit(this.status);
  }

  public get statusEnum() {
    return Status;
  }


}
