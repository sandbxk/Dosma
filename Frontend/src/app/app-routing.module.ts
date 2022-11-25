import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGroceryListsComponent} from "./components/user-grocery-lists/user-grocery-lists.component";

const routes: Routes = [
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full'},
  { path: 'dashboard', component: UserGroceryListsComponent },
  { path: 'grocery-list/:id', component: UserGroceryListsComponent },



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
