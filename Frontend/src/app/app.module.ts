import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { GroceryListComponent } from "./components/grocery-list/grocery-list.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatSnackBar } from "@angular/material/snack-bar";
import { CreateListDialogComponent } from "./dialogs/create-list-dialog/create-list-dialog.component";
import { UserGroceryListOverviewComponent } from "./components/user-grocery-list-overview/user-grocery-list-overview.component";
import { MatDialogModule } from "@angular/material/dialog";
import { MatCardModule } from "@angular/material/card";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatMenuModule } from "@angular/material/menu";
import { MatDividerModule } from "@angular/material/divider";
import { TruncatePipe } from "./util/truncate.pipe";
import { ConfirmationDialogComponent } from "./dialogs/confirmation-dialog/confirmation-dialog.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { EditListDialogComponent } from "./dialogs/edit-list-dialog/edit-list-dialog.component";
import { DragDropModule } from "@angular/cdk/drag-drop";
import { PendingChangesGuard } from "../services/PendingChanges.guard";
import { ItemCreatorComponent } from "./components/item-creator/item-creator.component";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatTooltipModule } from "@angular/material/tooltip";

@NgModule({
  declarations: [
    AppComponent,
    GroceryListComponent,
    CreateListDialogComponent,
    UserGroceryListOverviewComponent,
    TruncatePipe,
    ConfirmationDialogComponent,
    EditListDialogComponent,
    ItemCreatorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
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
    DragDropModule,
    MatCheckboxModule,
    MatToolbarModule,
    MatTooltipModule,
  ],
  providers: [MatSnackBar, PendingChangesGuard],
  bootstrap: [AppComponent]
})
export class AppModule {}
