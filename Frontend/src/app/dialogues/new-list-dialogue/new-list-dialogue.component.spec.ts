import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewListDialogueComponent } from './new-list-dialogue.component';

describe('NewListDialogueComponent', () => {
  let component: NewListDialogueComponent;
  let fixture: ComponentFixture<NewListDialogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewListDialogueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewListDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
