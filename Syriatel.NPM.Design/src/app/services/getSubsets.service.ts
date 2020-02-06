import { Injectable} from '@angular/core';
import { Subset } from '../extraClasses/subsets';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable} from 'rxjs/Observable';
import { DashboardResponse } from 'app/extraClasses/DashboardResponse';
import { UserInfo } from 'app/extraClasses/userInfo';
import { SessionStorageService } from 'ng2-webstorage';
import { ActionResponse } from 'app/extraClasses/ActionsResponse';

@Injectable()

export class SubsetsService {
  userAlias: string;
 
//private headers = new Headers({'Content-Type': 'application/json'});
   // URL to web api
   subsetsUrl: string;
   actionsUrl:string;

constructor(private http: Http,private storage: SessionStorageService) { }

getSubsets(universeName: string): Observable<DashboardResponse>  {
   this.subsetsUrl = 'http://seserv112/CreationTree/api/showsubsets?userAlias=';
  this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
  
  return this.http.get(this.subsetsUrl + this.userAlias + "&universe=" + universeName)
                 .map(sub => sub.json() as DashboardResponse)
                 .catch(this.handleError);


}

getActions(): Observable<ActionResponse>  {
  this.actionsUrl = 'http://seserv112/CreationTree/api/getactions?userAlias=';
 this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
 
 return this.http.get(this.actionsUrl + this.userAlias)
                .map(sub => sub.json() as ActionResponse)
                .catch(this.handleError);

}

 
private handleError(error: any): Promise<any> {
  //console.error('An error occurred', error); // for demo purposes only
  return Promise.reject(error.message || error);
}


}