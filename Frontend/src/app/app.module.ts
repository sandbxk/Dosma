import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GroceryListComponent } from './components/grocery-list/grocery-list.component';
import { ListItemComponent } from './components/list-item/list-item.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSnackBar} from "@angular/material/snack-bar";
import { CreateListDialogComponent } from './dialogs/create-list-dialog/create-list-dialog.component';
import { UserGroceryListsComponent } from './components/user-grocery-lists/user-grocery-lists.component';
import {MatDialogModule} from "@angular/material/dialog";
import {FlexLayoutModule} from "@angular/flex-layout";
import {MatCardModule} from "@angular/material/card";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatMenuModule} from "@angular/material/menu";
import {MatDividerModule} from "@angular/material/divider";
import {TruncatePipe} from "./util/truncate.pipe";
import { ConfirmationDialogComponent } from './dialogs/confirmation-dialog/confirmation-dialog.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import { EditListDialogComponent } from './dialogs/edit-list-dialog/edit-list-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    GroceryListComponent,
    ListItemComponent,
    CreateListDialogComponent,
    UserGroceryListsComponent,
    TruncatePipe,
    ConfirmationDialogComponent,
    EditListDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    FlexLayoutModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule,
    MatDialogModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
  ],
  providers: [MatSnackBar],
  bootstrap: [AppComponent]
})
export class AppModule { }
