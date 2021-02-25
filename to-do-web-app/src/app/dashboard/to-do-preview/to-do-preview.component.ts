import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ToDoList } from '../../shared/models/to-do-list.model';
import { ClipboardService } from 'ngx-clipboard';
import { ToDoService } from '../../core/to-do.service';

@Component({
  selector: 'app-to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
export class ToDoPreviewComponent implements OnInit {
  @Input() toDoList: ToDoList = new ToDoList();
  @Output() deleteToDoList: EventEmitter<string> = new EventEmitter();
  @Output() openToDoList: EventEmitter<ToDoList> = new EventEmitter();
  
  constructor(private clipboardService: ClipboardService, private toDoService: ToDoService) { }

  ngOnInit(): void {
  }

  copyContent() {
    this.toDoService.createShareToDoList(this.toDoList.id).subscribe(response => {
      this.clipboardService.copyFromContent(response);
    });
  }

  onDelete(id: any) {
    this.deleteToDoList.emit(id);
  }

  onClickList(toDoList: ToDoList) {
    this.openToDoList.emit(toDoList);
  }

  get completed() {
    return this.toDoList.toDoItems.filter(x => x.isDone);
  }

  get notCompleted() {
    return this.toDoList.toDoItems.filter(x => !x.isDone).sort((x1,x2) => x1.position - x2.position);
  }
}
