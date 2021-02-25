import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpBackend } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ToDoList } from '../shared/models/to-do-list.model';
import { ToDoItem } from '../shared/models/to-do-item.model';

const headers = new HttpHeaders().set('Content-Type', 'text/plain; charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class ToDoService {

  constructor(private http: HttpClient, private handler: HttpBackend) { }

  getToDoLists():Observable<ToDoList[]> {
    return this.http.get<ToDoList[]>(`${environment.dev.serverUrl}/to-do-lists`);
  }

  deleteToDoList(id: string):Observable<any> {
    return this.http.delete<any>(`${environment.dev.serverUrl}/to-do-lists/${id}`);
  }

  getToDoList(id: any):Observable<ToDoList> {
    return this.http.get<ToDoList>(`${environment.dev.serverUrl}/to-do-lists/${id}`);
  }

  editToDoList(toDoList: ToDoList):Observable<ToDoList> {
    return this.http.put<ToDoList>(`${environment.dev.serverUrl}/to-do-lists/${toDoList.id}`, toDoList);
  }

  editToDoItem(listId: string, itemId: any, toDoItem: ToDoItem):Observable<ToDoItem> {
    return this.http.put<ToDoItem>(`${environment.dev.serverUrl}/to-do-lists/${listId}/to-do-items/${itemId}`, toDoItem);
  }

  deleteToDoItem(listId: string, itemId: any):Observable<any> {
    return this.http.delete<any>(`${environment.dev.serverUrl}/to-do-lists/${listId}/to-do-items/${itemId}`);
  }

  createToDoItem(listId: any, toDoItem: ToDoItem):Observable<ToDoItem> {
    return this.http.post<ToDoItem>(`${environment.dev.serverUrl}/to-do-lists/${listId}/to-do-items`, toDoItem);
  }

  createToDoList(toDoList: ToDoList):Observable<ToDoList> {
    return this.http.post<ToDoList>(`${environment.dev.serverUrl}/to-do-lists`, toDoList);
  }

  updateItemPosition(listId: any, itemId: any, position: number):Observable<any> {
    return this.http.put<any>(`${environment.dev.serverUrl}/to-do-lists/${listId}/to-do-items/${itemId}/${position}`, {});
  }

  updateListPosition(listId: any, position: number):Observable<any> {
    return this.http.put<any>(`${environment.dev.serverUrl}/to-do-lists/${listId}/${position}`, {});
  }

  createShareToDoList(listId: any):Observable<any> {
    return this.http.post<any>(`${environment.dev.serverUrl}/to-do-lists/${listId}`, {}, {headers, responseType: 'text' as 'json'});
  }

  getSharedToDoList(listId: any):Observable<ToDoList> {
    this.http = new HttpClient(this.handler);
    return this.http.get<ToDoList>(`${environment.dev.serverUrl}/to-do-lists/${listId}/share`);
  }
}
