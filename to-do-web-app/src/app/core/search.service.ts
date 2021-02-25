import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ToDoList } from '../shared/models/to-do-list.model';

@Injectable({ 
    providedIn: 'root' 
})
export class SearchService {
    private subject = new Subject<any>();

    constructor(private http: HttpClient) {}

    getMessage(): Observable<any> {
        return this.subject.asObservable();
    }

    searchLists(listTitle?: string) {
        if (listTitle) {
            this.http.get<ToDoList[]>(`${environment.dev.serverUrl}/to-do-lists/search/${listTitle}`).subscribe(
                (filteredLists) => {
                    this.subject.next(filteredLists);
                }
            )
        } else {
            this.http.get<ToDoList[]>(`${environment.dev.serverUrl}/to-do-lists`).subscribe(
                (toDoLists) => {
                    this.subject.next(toDoLists);
                }
            )
        }        
    }
}