import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { DeactivateCounterResponse } from 'app/extraClasses/DeactivateCounterResponse';
import { AddSubsetResponse } from 'app/extraClasses/AddSubsetResponse';

@Component({
  selector: 'app-response-dialog',
  templateUrl: './response-dialog.component.html',
  styleUrls: ['./response-dialog.component.css']
})
export class ResponseDialogComponent implements OnInit {

  public receivedNode : any;
  public actionType : string;
  public file_name: string;
  private success : boolean;
  private Message: string;
  private DeactivateResponse : DeactivateCounterResponse;
  private AddSubsetResponse : AddSubsetResponse;
  
  constructor(public dialogRef: MatDialogRef<ResponseDialogComponent>) { }

  ngOnInit() {
    if(this.actionType == "Deactivate")
    {
      this.DeactivateResponse = <DeactivateCounterResponse>this.receivedNode;
      if(this.DeactivateResponse.success == 1)
      {
        this.success = true;
        this.Message = "Action Completed Successfully";
      }
      else
      {
        this.success = false;
        this.Message = this.DeactivateResponse.errorMessage;
      }
    }
    if(this.actionType == "AddSubset")
    {
      this.AddSubsetResponse = <AddSubsetResponse>this.receivedNode;
      if(this.AddSubsetResponse.success == 1)
      {
        this.success = true;
        this.Message = "Action Completed Successfully";
      }
      else
      {
        this.success = false;
        this.Message = this.AddSubsetResponse.errorMessage;
      }
    }

  }

  reload() {
    window.location.reload();
  }

}
