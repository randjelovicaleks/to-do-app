import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ToDoList } from '../shared/models/to-do-list.model';

@Injectable({ 
    providedIn: 'root' 
})
export class MessageService {
    private subject = new Subject<any>();

    constructor(private http: HttpClient) {}

    getMessage(): Observable<any> {
        return this.subject.asObservable();
    }

    createList() {
        this.http.post<ToDoList>(`${environment.dev.serverUrl}/to-do-lists`, new ToDoList()).subscribe(
            (res) => {
                this.subject.next(res);
            }
        );
    }
}