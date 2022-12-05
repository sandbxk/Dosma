import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGroceryListOverviewComponent } from "./components/user-grocery-list-overview/user-grocery-list-overview.component";
import { GroceryListComponent } from "./components/grocery-list/grocery-list.component";
import { PendingChangesGuard } from "../services/PendingChanges.guard";
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { ContactUsComponent } from './components/contact-us/contact-us.component';
import { EditorProfileComponent } from './components/editor-profile/editor-profile.component';

const routes: Routes = [
  { path: 'dashboard', component: UserGroceryListOverviewComponent },
  { path: 'grocery-list/:id', component: GroceryListComponent, canDeactivate: [PendingChangesGuard] },
  { path: 'about', component: AboutUsComponent },
  { path: 'contact', component: ContactUsComponent },
  { path: 'account', component: EditorProfileComponent },
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
