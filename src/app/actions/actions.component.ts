import { Component, OnInit } from '@angular/core';
import { SessionStorageService } from 'ng2-webstorage';
import { Router } from '@angular/router';
import { ANIMATION_TYPES } from 'ngx-loading';
import { Subscription } from 'rxjs';
import { SubsetsService } from 'app/services/getSubsets.service';
import { Action } from 'app/extraClasses/Actions';
import { UserInfo } from 'app/extraClasses/userInfo';
declare var jquery: any;
declare var $: any;

declare interface TableData {
  headerRow: string[];
  dataRows: string[][];
}

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.css']
})
export class ActionsComponent implements OnInit {

  public loading = false;
  busy: Subscription;
  actions: Action[];

  public tableData1: TableData;

  public className:string;
  
  constructor(private ActionService: SubsetsService, private storage: SessionStorageService,private _router: Router) { }

  ngOnInit() {

      if (!this.storage.retrieve('currentUser')) {
        $('.actionsData').empty();
        this._router.navigate(['/login']);
      }
       this.tableData1 = {
         headerRow: ['Action Name', 'Subset Id', 'Counter Id', 'Process code', 'Date', 'Done In Counters DB', 'Done In Business Objects'],
        dataRows: [
             ['', '', '', '', '', '', ''],
         ]
     };
     console.log(this.tableData1);
      this.getActions();
      
  }

  getActions(): void {
    
      this.loading = true;
      this.className = "test";
      this.busy = this.ActionService.getActions().subscribe(values => {
        console.log('this.values', values);
      this.actions = values.data;
      this.FillTable();
      this.loading = false;
      });
  }

  FillTable(): void {
    
    this.tableData1 = {
      headerRow: [ 'Action Name', 'Subset Id', 'Counter Id', 'Process code', 'Date', 'Done In Counters DB', 'Done In Business Objects'],
      dataRows: []
    };
    for (let action of this.actions) 
    {
      var row : string[];
      row = [];
      if(action.ActionName == 'DEACTIVATE_COUNTER_IN_DB')
      {
        row.push('Deactivate Counter');
      }
      else if(action.ActionName == 'ADD_SUBSET_TO_DB')
      {
        row.push('Add Subset');
      }
      else if(action.ActionName == 'ADD_COUNTER_TO_DB')
      {
        row.push('Add Counter');
      }
      else if(action.ActionName == 'CHANGE_COUNTER_PATH_IN_DB')
      {
        row.push('Change Counter Path');
      }
      else if(action.ActionName == 'ADD_DIMENTIONS_TO_DB')
      {
        row.push('Add Dimention');
      }
      row.push(action.SubsetId);
      row.push(action.CounterId);
      row.push(action.FileName);
      row.push(action.Date);

      if(action.SuccessInDB == 'Y ')
      {
        row.push('successed');
      }
      else
      {
        row.push('failed');
      }
      
      if(action.SuccessInBO == 'Y')
      {
        row.push('successed');
      }
      else
      {
        row.push('failed');
      }

       this.tableData1.dataRows.push(row);
    }
    
  }

}
