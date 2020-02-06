import { Injectable} from '@angular/core';
import { Node } from '../extraClasses/nodes';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable} from 'rxjs/Observable';
import { TreeResponse } from 'app/extraClasses/TreeResponse';
import { SessionStorageService } from 'ng2-webstorage';
import { UserInfo } from 'app/extraClasses/userInfo';

@Injectable()

export class NodesService{
  userAlias: string;

//private headers = new Headers({'Content-Type': 'application/json'});
private TreeUrl = 'http://seserv112/CreationTree/api/showtree?userAlias=';

constructor(private http: Http,private storage: SessionStorageService) { }

getUniverseTree(universe:string): Observable<TreeResponse>  {
  this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
  console.log(this.TreeUrl + (this.userAlias + "&universe=" + universe));
    return this.http.get(this.TreeUrl + (this.userAlias + "&universe=" + universe))
    .map(response => response.json() as TreeResponse)
    .catch(this.handleError);
}

 
private handleError(error: any): Promise<any> {
  console.error('An error occurred', error); // for demo purposes only
  return Promise.reject(error.message || error);
}


}