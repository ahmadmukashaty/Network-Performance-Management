import { Logs } from '../extraClasses/logs';
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig, MatDialog } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { SubsetCounter } from 'app/extraClasses/subsetCounter';
import { RequestOptions, Headers, Http } from '@angular/http';
import { Router } from '@angular/router';
import { DeactivateCounterPost } from 'app/extraClasses/DeactivateCounterPost';
import { DeactivateCounterResponse } from 'app/extraClasses/DeactivateCounterResponse';
import { ResponseDialogComponent } from 'app/response-dialog/response-dialog.component';
import { UserInfo } from 'app/extraClasses/userInfo';
import { SessionStorageService } from 'ng2-webstorage';
import { MessageComponent } from 'app/message/message.component';
import { ChangePath } from 'app/extraClasses/changePath';
import { ChangeCounterPathPost } from 'app/extraClasses/ChangeCounterPathPost';
import { ChangePathResponse } from 'app/extraClasses/ChangePathResponse';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
  
  newPath: string;
  addPath: string;
  loading: boolean;
  date: string;
  dateT: string;
  dateD: string;
  date1:  Date;
  username: string;
  log: Logs;
  public changePath: ChangePath;
  public receivedNode: SubsetCounter;
  private postNode: DeactivateCounterPost;
  private changePathPost: ChangeCounterPathPost;
  public enableMoveCounterBtn: boolean;
  private isLeaf = false;
  private deactivateResponse: DeactivateCounterResponse;
  private changePathResponse: ChangePathResponse;
  private valueType: string;
  constructor(public dialogRef: MatDialogRef<DialogComponent>,  private http: Http, private _router: Router, public dialog: MatDialog, public dialog2: MatDialog, private storage: SessionStorageService) { }
  ngOnInit() {
    if (this.receivedNode.tableCounterName === undefined) {
      this.isLeaf = false;
    }else {
      this.isLeaf = true;
    }
    if(this.receivedNode.valueType == 68)
    {
      this.valueType = "Dimention";
    }
    else
    {
      this.valueType = "Measurement";
    }
  }
  getUser(): string {
    this.username = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;;
    return this.username ;
  }
  DeactivateService() {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });
    const body = JSON.stringify(this.postNode);
    return this.http.post('http://seserv112/CreationTree/api/deactivate', body, options ).map(res => res.json());
  }


  DeactivateCounter(): any {
    this.loading = true;
    this.postNode = new DeactivateCounterPost();

    this.date1 =  new Date();
    this.dateD = this.date1.toLocaleDateString();
    this.dateT = this.date1.toLocaleTimeString();
    this.date = this.dateD + ' ' + this.dateT;

    this.postNode.userAlias = this.getUser();
    if (this.receivedNode.counterID == null) {
      this.postNode.file_name = this.receivedNode.value + '_' + this.date;
    } else {
      this.postNode.file_name = this.receivedNode.counterID + '_' + this.date;
    }
    this.postNode.data = this.receivedNode;
  this.DeactivateService().subscribe( response => {
        this.deactivateResponse = response;
        this.openDialog(this.deactivateResponse);
      });

      this.loading = false;
  }

  SetPath() {
        if ( this.addPath != null) {
          this.newPath = this.receivedNode.path + '/' + this.addPath;
          this._router.navigate(['../dashboard', {transPath: this.newPath}]);
        } else {
        this._router.navigate(['../dashboard', {transPath: this.receivedNode.path}]);
      }
  }

  openDialog(selectedNode: DeactivateCounterResponse) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<ResponseDialogComponent> = this.dialog.open(ResponseDialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
    dialogRef.componentInstance.file_name = this.postNode.file_name;
    dialogRef.componentInstance.actionType = 'Deactivate';
  }

  openDialog2(selectedNode: ChangePathResponse) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<ResponseDialogComponent> = this.dialog2.open(ResponseDialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
    dialogRef.componentInstance.file_name = this.changePathPost.file_name;
    dialogRef.componentInstance.actionType = 'ChangePath';
  }

  ChangePathService() {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    const options = new RequestOptions({ headers: headers });
    const body = JSON.stringify(this.changePathPost);
    return this.http.post('http://seserv112/CreationTree/api/changecounterpath', body, options ).map(res => res.json());
  }

  ChangePathResponse() {
    this.changePath = new ChangePath();
    this.changePath.actionType = "changePathAction";
    this.changePath.counter = this.receivedNode;
    this.changePath.newPath = null;
    this.dialogRef.close(this.changePath);
  }

  ChangeCounterPath()
  {
      if ( this.addPath != null) 
      {
        this.changePath.newPath = this.InvertPath (this.receivedNode.path + '/' + this.addPath);
      } 
      else
      {
        this.changePath.newPath = this.InvertPath (this.receivedNode.path);
      }
      this.loading = true;
      this.changePathPost = new ChangeCounterPathPost();

      this.date1 =  new Date();
      this.dateD = this.date1.toLocaleDateString();
      this.dateT = this.date1.toLocaleTimeString();
      this.date = this.dateD + ' ' + this.dateT;

      this.changePathPost.userAlias = this.getUser();
      if (this.changePath.counter.counterID == null) {
        this.changePathPost.file_name = this.changePath.counter.value + '_' + this.date;
      } else {
        this.changePathPost.file_name = this.changePath.counter.counterID + '_' + this.date;
      }
      this.changePathPost.data = this.changePath;
      this.ChangePathService().subscribe( response => {

          this.changePathResponse = (<ChangePathResponse>response);
          this.dialogRef.close(null);
          this.openDialog2(this.changePathResponse);
      });
      this.loading = false;
  }

  InvertPath(path:string):string
  {
      if(path != null && path != "undefined")
      {
          var tokens:string[] = path.split('/');
          tokens = tokens.reverse();
          var firstIteration:boolean = true;
          var newPath:string = "";
          for(let subPath of tokens)
          {
              if(firstIteration)
              {
                  newPath+= subPath;
                  firstIteration = false;
              }
              else
              {
                  newPath+= ("/"+subPath);
              }
          }
          return newPath;
      }
      return path;
  }

}
