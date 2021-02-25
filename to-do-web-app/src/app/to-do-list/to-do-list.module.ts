import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { ToDoListComponent } from '../to-do-list/to-do-list.component';
import { ToDoItemComponent } from './to-do-item/to-do-item.component';

import { DndModule } from 'ng2-dnd';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share.component';

@NgModule({
  declarations: [
    ToDoListComponent,
    ToDoItemComponent,
    ToDoListShareComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    FormsModule,
    DndModule,
  ],
  providers: [],
  bootstrap: [ToDoListComponent],
  exports: [DndModule]
})
export class ToDoListModule { }