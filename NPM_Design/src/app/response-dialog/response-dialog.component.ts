import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { DeactivateCounterResponse } from 'app/extraClasses/DeactivateCounterResponse';
import { AddSubsetResponse } from 'app/extraClasses/AddSubsetResponse';
import { DimentionResponse } from 'app/extraClasses/DimentionResponse';
import { ChangePathResponse } from 'app/extraClasses/ChangePathResponse';

@Component({
  selector: 'app-response-dialog',
  templateUrl: './response-dialog.component.html',
  styleUrls: ['./response-dialog.component.css']
})
export class ResponseDialogComponent implements OnInit {

  public receivedNode : any;
  public actionType : string;
  public file_name: string;
  public subsetID: string;
  private success : boolean;
  private Message: string;
  private DeactivateResponse : DeactivateCounterResponse;
  private AddSubsetResponse : AddSubsetResponse;
  private DimentionResponse : DimentionResponse;
  private ChangePathResponse : ChangePathResponse;
  private unactivateSubset : boolean;
  constructor(public dialogRef: MatDialogRef<ResponseDialogComponent>) { }

  ngOnInit() {
    console.log("in dialog");
    console.log(this.ChangePathResponse);
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
      this.unactivateSubset = false;
      this.AddSubsetResponse = <AddSubsetResponse>this.receivedNode;
      if(this.AddSubsetResponse.success == 1)
      {
        this.success = true;
        this.Message = "Action Completed Successfully";
      }
      else
      {
        this.success = false;
        if(this.AddSubsetResponse.errorMessage == "UnactivatedSubset")
        {
          this.unactivateSubset = true;
          this.Message = "Subset " + this.subsetID + " is not activated please contact with Core Operation team to activate";
        }
        else
        {
          this.Message = this.AddSubsetResponse.errorMessage;
        }
        
      }
    }
    if(this.actionType == "GetDimentions")
    {
      this.unactivateSubset = false;
      this.DimentionResponse = <DimentionResponse>this.receivedNode;
      if(this.DimentionResponse.success == -1)
      {
        this.success = false;
        if(this.DimentionResponse.errorMessage == "UnactivatedSubset")
        {
          this.unactivateSubset = true;
          this.Message = "Subset " + this.subsetID + " is not activated please contact with Core Operation team to activate";
        }
        else
        {
          this.Message = this.DimentionResponse.errorMessage;
        }
      }
    }
    if(this.actionType == "ChangePath")
    {
      this.ChangePathResponse = <ChangePathResponse>this.receivedNode;
      if(this.ChangePathResponse.success == 1)
      {
        this.success = true;
        this.Message = "Action Completed Successfully";
      }
      else
      {
        this.success = false;
        this.Message = this.ChangePathResponse.errorMessage;
      }
    }

  }

  reload() {
    window.location.reload();
  }

}
