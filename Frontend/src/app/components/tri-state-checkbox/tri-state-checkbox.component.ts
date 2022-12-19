import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Status } from '../../interfaces/StatusEnum';
import {
  animate,
  keyframes,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';

/**
 * A component that displays a checkbox with three states - 'done', 'skipped', and unchecked.
 */
@Component({
  selector: 'app-tri-state-checkbox',
  templateUrl: './tri-state-checkbox.component.html',
  styleUrls: ['tri-state-checkbox.component.scss'],
  animations: [
    trigger('inOutAnimation', [
      state('in', style({ opacity: 1 })),
      transition(':enter', [
        animate(
          100,
          keyframes([
            style({ opacity: 0, offset: 0 }),
            style({ opacity: 0.25, offset: 0.25 }),
            style({ opacity: 0.5, offset: 0.5 }),
            style({ opacity: 0.75, offset: 0.75 }),
            style({ opacity: 1, offset: 1 }),
          ])
        ),
      ]),
      transition(':leave', [
        animate(
          100,
          keyframes([
            style({ opacity: 1, offset: 0 }),
            style({ opacity: 0.75, offset: 0.25 }),
            style({ opacity: 0.5, offset: 0.5 }),
            style({ opacity: 0.25, offset: 0.75 }),
            style({ opacity: 0, offset: 1 }),
          ])
        ),
      ]),
    ]),
  ],
})
export class TriStateCheckboxComponent {
  constructor() {}

  ngOnInit(): void {}

  @Output() statusChangedEvent = new EventEmitter<Status>();

  /**
   * The state of the checkbox.
   */
  @Input() status: Status = Status.Unchecked;
  allStatus = Status;

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
    this.statusChangedEvent.emit(this.status);
  }

  public get statusEnum(): typeof Status {
    return Status;
  }
}
