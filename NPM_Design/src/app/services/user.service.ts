/*import { Injectable, Output, EventEmitter } from '@angular/core';
import { User } from '../_models/user';

@Injectable()
export class UserService {
    private user:User;

    @Output() public userChangeEvent = new EventEmitter();

    setUser(user:User) {
        this.user = user;
        this.userChangeEvent.emit(user);
    }

    getUser():User {
        return this.user;
    }
}*/
import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';


import { User } from '../_models/index';
import * as globals             from './global';
@Injectable()
export class UserService {
    constructor(private http: Http) { }

    getAll() {
        return this.http.get(globals.ApiUrl +'/users', this.jwt()).map((response: Response) => response.json());
    }

    getById(id: number) {
        return this.http.get(globals.ApiUrl + '/users/' + id, this.jwt()).map((response: Response) => response.json());
    }

    create(user: User) {
        return this.http.post(globals.ApiUrl +'/Authentication/Register/', user, this.jwt());
    }

   /* update(user: User) {
        return this.http.put(this.config.apiUrl + '/users/' + user.id, user, this.jwt());
    }*/

    delete(id: number) {
        return this.http.delete(globals.ApiUrl + '/users/' + id, this.jwt());
    }

    // private helper methods

    private jwt() {
        let headers = new Headers();
        // headers.append('Content-Type', 'application/x-www-form-urlencoded'); 
         headers.append('Content-Type', 'application/json');
         headers.append('Access-Control-Allow-Headers', '*');
         headers.append('Access-Control-Allow-Origin', '*');
        
         return new RequestOptions({ headers: headers });
        // create authorization header with jwt token
        /*
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            //headers.append('Content-Type', 'application/json');
            return new RequestOptions({ headers: headers });
        }*/
        
    }
}