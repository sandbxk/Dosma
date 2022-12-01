import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGroceryListOverviewComponent} from "./components/user-grocery-list-overview/user-grocery-list-overview.component";
import {GroceryListComponent} from "./components/grocery-list/grocery-list.component";
import {PendingChangesGuard} from "../services/PendingChanges.guard";

const routes: Routes = [
  { path: 'dashboard', component: UserGroceryListOverviewComponent },
  { path: 'grocery-list/:id', component: GroceryListComponent, canDeactivate: [PendingChangesGuard] },
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full'},



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
