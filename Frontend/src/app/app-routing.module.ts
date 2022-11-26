import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGroceryListsComponent} from "./components/user-grocery-lists/user-grocery-lists.component";
import {GroceryListComponent} from "./components/grocery-list/grocery-list.component";

const routes: Routes = [
  { path: 'dashboard', component: UserGroceryListsComponent },
  { path: 'grocery-list/:id', component: GroceryListComponent },
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full'},



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
