import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Todo{
    completed: boolean,
    title: string,
    id?: number
  }

@Injectable({providedIn: 'root'})
export class TodosService{

    constructor(private httpClient: HttpClient){}

     addTodo(todo: Todo) : Observable<Todo> {
          return this.httpClient.post<Todo>('https://jsonplaceholder.typicode.com/todos', todo);
    }

    fetchTodos() : Observable<Todo[]>{
        return this.httpClient.get<Todo[]>('https://jsonplaceholder.typicode.com/todos?_limit=2');
    }
}
