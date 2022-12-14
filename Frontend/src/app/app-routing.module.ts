import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGroceryListOverviewComponent } from "./components/user-grocery-list-overview/user-grocery-list-overview.component";
import { GroceryListComponent } from "./components/grocery-list/grocery-list.component";
import {AuthGuardService} from "../services/authGuard.service";
import {HomePageComponent} from "./components/home-page/home-page.component";

const routes: Routes = [
  { path: 'dashboard', component: UserGroceryListOverviewComponent, canActivate: [AuthGuardService] },
  { path: 'grocery-list/:id', component: GroceryListComponent, canActivate: [AuthGuardService] },
  { path: '', component: HomePageComponent },
  { path: '**', redirectTo: '', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
