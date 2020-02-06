import { Injectable }  from '@angular/core';
import { Headers, Http }  from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { User }        from '../_models/user'
import * as globals       from './global';

@Injectable()
export class UsersService {
 /* public USERS: User[] = [
      { UserId: 1, UserName: 'Ayat.Hamwi', EMail: 'Ayat.Hamwi@syriatel.sy', Token:'' },
      { UserId: 2, UserName: 'Sarah.Shalfoun', EMail: 'Sarah.Shalfoun@syriatel.sy', Token:'' },
      { UserId: 3, UserName: 'Razan.Shukairi', EMail: 'Razan.Shukairi@syriatel.sy', Token:'' },
      { UserId: 4, UserName: 'Rana.Dahbar', EMail: 'Rana.Dahbar@syriatel.sy', Token:'' },
      { UserId: 5, UserName: 'Weam.Baladi', EMail: 'Weam.Baladi@syriatel.sy', Token:'' },
      { UserId: 6, UserName: 'Ammar.Darweesh', EMail: 'Ammar.Darweesh@syriatel.sy', Token:'' },
      { UserId: 7, UserName: 'Sofia.Barakeh', EMail: 'Sofia.Barakeh@syriatel.sy', Token:'' },
      { UserId: 8, UserName: 'Nour.Ali', EMail: 'Nour.Ali@syriatel.sy', Token:'' },
      { UserId: 9, UserName: 'Mohammad.Ibrahim', EMail: 'Mohammad.Ibrahim@syriatel.sy', Token:'' },
      { UserId: 10, UserName: 'Faten.Zahra', EMail: 'Faten.Zahra@syriatel.sy', Token:'' },
      { UserId: 11, UserName: 'Rabee.Abouras', EMail: 'Rabee.Abouras@syriatel.sy', Token:'' },
      { UserId: 12, UserName: 'Raghad.Hammami', EMail: 'Raghad.Hammami@syriatel.sy', Token:'' }
  ];*/

  constructor(private http: Http) { }

  private getHeaders(){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return headers;
  }

  /*getUsers(): Promise<User[]> {
    return this.http.get(globals.ApiUrl + 'api/Users', {headers: this.getHeaders()})
               .toPromise()
               .then(response => response.json() as User[])
               .catch(this.handleError);               
  }

  getUser(id: number): Promise<User> {
    const url = `${globals.ApiUrl + 'api/Users'}/${id}`;
    return this.http.get(url, {headers: this.getHeaders()})
               .toPromise()
               .then(response => response.json() as User)
               .catch(this.handleError);               
  }

  delete(id: number): Promise<void> {
    const url = `${globals.ApiUrl + 'api/Users'}/${id}`;
    return this.http.delete(url, {headers: this.getHeaders()})
      .toPromise()
      .then(() => null)
      .catch(this.handleError);
  }

  create(user: User): Promise<User> {
    return this.http
      .post(globals.ApiUrl + 'api/Users', JSON.stringify(user), {headers: this.getHeaders()})
      .toPromise()
      .then(res => res.json())
      .catch(this.handleError);
  }

  update(user: User): Promise<User> {
    const url = `${globals.ApiUrl + 'api/Users'}/${user.UserId}`;
    return this.http
      .put(url, JSON.stringify(user), {headers: this.getHeaders()})
      .toPromise()
      .then(() => user)
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }*/

}