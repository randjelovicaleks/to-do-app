import { Component, OnInit } from '@angular/core';
import { ToDoList } from '../../shared/models/to-do-list.model';
import { ToDoService } from '../../core/to-do.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-to-do-list-share',
  templateUrl: './to-do-list-share.component.html',
  styleUrls: ['./to-do-list-share.component.css']
})
export class ToDoListShareComponent implements OnInit {
  toDoList: ToDoList = new ToDoList();
  shareToDoListId: any;
  sub: any;

  constructor(private toDoService: ToDoService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(params => {
      this.shareToDoListId = params['id'];
    });

    this.toDoService.getSharedToDoList(this.shareToDoListId).subscribe(response => {
      this.toDoList = response;
    });
  }

  get completed() {
    return this.toDoList.toDoItems.filter(x => x.isDone);
  }

  get notCompleted() {
    return this.toDoList.toDoItems.filter(x => !x.isDone).sort((x1,x2) => x1.position - x2.position);
  }

}
