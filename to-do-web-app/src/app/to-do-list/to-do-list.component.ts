import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToDoService } from '../core/to-do.service';
import { ToDoList } from '../shared/models/to-do-list.model';

import { MessageService } from '../core/message.service';
import { Subscription } from 'rxjs';

import { Router } from '@angular/router';
import {HostListener} from "@angular/core";
import { ToDoItem } from '../shared/models/to-do-item.model';

@Component({
  selector: 'app-to-do-list',
  templateUrl: './to-do-list.component.html',
  styleUrls: ['./to-do-list.component.css']
})
export class ToDoListComponent implements OnInit, OnDestroy {
  sub: any;
  toDoList: ToDoList = new ToDoList();
  subscription: Subscription = new Subscription();
  showValidation: boolean = false;

  constructor(private route: ActivatedRoute, private toDoService: ToDoService, private messageService: MessageService, private router: Router) { }
  
  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.toDoList.id = params['id'];
    });

    if (this.toDoList.id) {
      this.getToDoList();
    } 

    this.subscription = this.messageService.getMessage().subscribe((result) => {
      this.toDoList = result;
    });

  }

  editToDoList() {
    if (this.toDoList.id) {
      this.toDoService.editToDoList(this.toDoList).subscribe();
    } else {
      this.toDoService.createToDoList(this.toDoList).subscribe(
        (response) => {
          this.toDoList = response;
          this.router.navigate([`/to-do-list/${this.toDoList.id}`]);
        }
      );
    }
  }

  getToDoList() {
    this.toDoService.getToDoList(this.toDoList.id).subscribe(
      (toDoList) => {
        this.toDoList = toDoList;
    })
  }

  get completed() {
    return this.toDoList.toDoItems.filter(x => x.isDone).sort((x1,x2) => x1.position - x2.position);
  }

  get notCompleted() {
    return this.toDoList.toDoItems.filter(x => !x.isDone).sort((x1,x2) => x1.position - x2.position);
  }

  onMove(listId: any, $event: any, toDoItem: ToDoItem) {
    this.toDoService.updateItemPosition(listId, $event.dragData.id, toDoItem.position).subscribe(
      () => {
        this.getToDoList();
      }
    );
  }

  addReminder() {
    if (this.toDoList.id) {
      this.showValidation = true;
      if (this.toDoList.reminderDateTime) {
        this.toDoList.isReminded = false;
        this.toDoService.editToDoList(this.toDoList).subscribe(
          () => {
            this.getToDoList();
            this.showValidation = false;
          });
      } 
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
