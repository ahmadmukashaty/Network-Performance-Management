import { Injectable } from '@angular/core';
import { Headers, Http , Response, RequestOptions} from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { User } from '../_models/user';
import * as globals from './global';
import {Observable} from 'rxjs/Observable';
import {LocalStorageService, SessionStorageService} from 'ng2-webstorage';
import { Router, ActivatedRoute } from '@angular/router';
@Injectable()
export class AuthenticationService {
    returnUrl: string;
    resp: string;
    constructor(private http: Http, private storage: SessionStorageService,
        private router: Router, private route: ActivatedRoute
    ) {this.returnUrl = this.route.snapshot.params['returnUrl'] || '/' }

    private getHeaders(){
        let headers = new Headers();
       // headers.append('Content-Type', 'application/x-www-form-urlencoded');
        headers.append('Content-Type', 'application/json');
        headers.append('Access-Control-Allow-Headers', '*');
        headers.append('Access-Control-Allow-Origin', '*');

        return new RequestOptions({ headers: headers });
    }

 /*   login(username: string, password: string): Promise<User> {
        //var data = JSON.stringify({ username: username, password: password, grant_type: 'password' });
        var data = "grant_type=password&username=" + username + "&password=" + password;
        return this.http.post(globals.ApiUrl + 'Token', data, {headers: this.getHeaders()})
                 .toPromise()
                 .then(response => {
                    let tokenResponse = response.json();
                    let token: string;
                    let Ident = '';
                    let FirstName = '';
                    let LastName = '';
                    let user: User;
                    if (tokenResponse) {
                        token = tokenResponse['access_token'];
                        Ident = tokenResponse['userId'];
                        FirstName = tokenResponse['firstName'];
                        LastName = tokenResponse['lastName'];
                        user = new User();
                        user.UserId = parseInt(Ident);
                        user.UserName = FirstName + ' ' + LastName;
                        user.EMail = username;
                        user.Token = token;
                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem('currentUser', JSON.stringify(user));
                        console.log('Id: ' + user.UserId + ', Name= ' + user.UserName);
                    }
                    else
                    {
                        console.log('Con not set user information!');
                    }
                    return user;
                 })
                 .catch(this.handleError);
    }*/
    login(username: string, password: string) {
        return this.http.post(globals.ApiUrl +  '/Authentication/Authenticate/',
         { username: username, password: password }, this.getHeaders())
         
    }



    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }
 private handleError(error: any): Promise<any> {
      console.error('An error occurred', error); // for demo purposes only
      return Promise.reject(error.message || error);
    }
}
