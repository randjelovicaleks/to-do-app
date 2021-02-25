import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ToDoItem } from '../../shared/models/to-do-item.model';
import { ToDoService } from '../../core/to-do.service';

import { MessageService } from '../../core/message.service';
import { Subscription } from 'rxjs';

import { Router } from '@angular/router';
import {HostListener} from "@angular/core";

@Component({
  selector: 'app-to-do-item',
  templateUrl: './to-do-item.component.html',
  styleUrls: ['./to-do-item.component.css']
})
export class ToDoItemComponent implements OnInit, OnDestroy {
  @Input() toDoItem: ToDoItem = new ToDoItem();
  @Input() listId: any;
  @Output() onItemChange: EventEmitter<ToDoItem> = new EventEmitter();
  subscription: Subscription = new Subscription();

  constructor(private toDoService: ToDoService, private messageService: MessageService, private router: Router) { }

  ngOnInit(): void {
    
    this.subscription = this.messageService.getMessage().subscribe(list => {
      if (list) {
        this.toDoService.createToDoItem(list.id, this.toDoItem).subscribe(
          (toDoItem) => {
            this.toDoItem = toDoItem;
            this.router.navigate([`/to-do-list/${list.id}`]);
          }
        );
      }     
    })
  }

  editItem() {
    if (this.toDoItem.id) {
      this.toDoService.editToDoItem(this.listId, this.toDoItem.id, this.toDoItem).subscribe(
        (editedToDoItem) => {
          this.toDoItem = editedToDoItem;
          this.onItemChange.emit();
        }
      );
    } else {
      if (this.listId) {
        this.toDoService.createToDoItem(this.listId, this.toDoItem).subscribe(
          () => {
            this.toDoItem = new ToDoItem();
            this.onItemChange.emit();
          }
        );  
      } else {         
        this.messageService.createList();                    
      }
    }   
  }

  deleteItem() {
    this.toDoService.deleteToDoItem(this.listId, this.toDoItem.id).subscribe(
      () => {
        this.onItemChange.emit();
      }
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
