import { ToDoItem } from './to-do-item.model';

export class ToDoList {
    id?: string;
    title: string = '';
    position: number = 0;
    toDoItems: ToDoItem[] = new Array();
    isReminded: boolean = true;
    reminderDateTime?: Date;
}
