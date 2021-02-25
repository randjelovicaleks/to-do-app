import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { ToDoListComponent } from './to-do-list/to-do-list.component';
import { ToDoListShareComponent } from '../app/to-do-list/to-do-list-share/to-do-list-share.component';

import { AuthGuard } from '../app/auth/auth-guard';

const routes: Routes = [
  { 
    path: '', 
    component: DashboardComponent 
  },
  { 
    path: 'to-do-list', 
    component: ToDoListComponent,
    canActivate: [AuthGuard],
    data: { expectedScopes: 'create:to-do-list create:to-do-item' }
  },
  { 
    path: 'to-do-list/:id', 
    component: ToDoListComponent,
    canActivate: [AuthGuard],
    data: { expectedScopes: 'update:to-do-list update:to-do-item' }
  },
  { 
    path: 'to-do-list/share/:id', 
    component: ToDoListShareComponent,
  },
  { 
    path: "**", 
    redirectTo: "/", 
    pathMatch: "full" 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
