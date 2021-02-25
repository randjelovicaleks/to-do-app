import { Component, OnInit } from '@angular/core';
import { ToDoService } from '../core/to-do.service';
import { ToDoList } from '../shared/models/to-do-list.model';

import { Router } from '@angular/router';
import { SearchService } from '../core/search.service';
import { Subscription } from 'rxjs';

import { AuthService } from '@auth0/auth0-angular';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  toDoLists: ToDoList[] = new Array();
  subscription: Subscription = new Subscription();
  
  constructor(private toDoService: ToDoService, private router: Router, private searchService: SearchService, private auth: AuthService) { }

  ngOnInit(): void {
    const helper = new JwtHelperService();
    this.auth.getAccessTokenSilently().subscribe(token => {
      localStorage.setItem('scopes', JSON.stringify(helper.decodeToken(token).scope.split(' ')));     
    })

    this.getToDoLists();

    this.subscription = this.searchService.getMessage().subscribe(filteredLists => {
      if (filteredLists) {
        this.toDoLists = filteredLists;
      }
    })
  }

  getToDoLists() {
    this.toDoService.getToDoLists().subscribe(toDoLists => {
      this.toDoLists = toDoLists;
    });
  }

  deleteToDoList(id: string) { 
    this.toDoService.deleteToDoList(id).subscribe(
      () => {
        this.getToDoLists();
      }
    );   
  }

  openToDoList(toDoList: ToDoList) {
    if (!toDoList.isReminded) {
      toDoList.isReminded = true;
      toDoList.reminderDateTime = new Date();

      this.toDoService.editToDoList(toDoList).subscribe(
        () => {
          this.router.navigate([`/to-do-list/${toDoList.id}`]);
      });
    } else {
      this.router.navigate([`/to-do-list/${toDoList.id}`]);
    }
        
  }

  onMove($event: any, toDoList: ToDoList) {
    this.toDoService.updateListPosition($event.dragData.id, toDoList.position).subscribe(
      () => {
        this.getToDoLists();
      }
    );
  }

  get withReminder() {
    return this.toDoLists.filter(x => !x.isReminded);
  }

  get withoutReminder() {
    return this.toDoLists.filter(x => x.isReminded);
  }

}
