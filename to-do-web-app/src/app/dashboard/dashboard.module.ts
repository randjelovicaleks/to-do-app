import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { DashboardComponent } from './dashboard.component';
import { ToDoPreviewComponent } from './to-do-preview/to-do-preview.component';

import { DndModule } from 'ng2-dnd';
import { ClipboardModule } from 'ngx-clipboard';

@NgModule({
  declarations: [
    DashboardComponent,
    ToDoPreviewComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpClientModule,
    DndModule.forRoot(),
    ClipboardModule
  ],
  providers: [],
  bootstrap: [DashboardComponent],
})
export class DashboardModule { }