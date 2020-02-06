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
  public receivedNode: SubsetCounter;
  private postNode: DeactivateCounterPost;
  private isLeaf = false;
  private deactivateResponse: DeactivateCounterResponse;
  constructor(public dialogRef: MatDialogRef<DialogComponent>,  private http: Http, private _router: Router, public dialog: MatDialog, private storage: SessionStorageService) { }
  ngOnInit() {
    
    if (this.receivedNode.tableCounterName === undefined) {
      this.isLeaf = false;
    }else {
      this.isLeaf = true;
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
    console.log(body);
    return this.http.post('http://seserv112/CreationTree/api/deactivate', body, options ).map(res => res.json());
  }


  DeactivateCounter(): any {
    this.loading = true;
    this.postNode = new DeactivateCounterPost();

    console.log('Deactivate counter test ......');
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


}
