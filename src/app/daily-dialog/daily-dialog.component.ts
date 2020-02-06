import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { SubsetCounter } from 'app/extraClasses/subsetCounter';
import { RequestOptions, Headers, Http } from '@angular/http';
import { Router } from '@angular/router';
import { Daily } from 'app/extraClasses/daily';

@Component({
  selector: 'app-daily-dialog',
  templateUrl: './daily-dialog.component.html',
  styleUrls: ['./daily-dialog.component.css']
})
export class DailyDialogComponent implements OnInit {
  public receivedNode: Daily;
  private isLeaf: boolean = false;
  constructor(public dialogRef: MatDialogRef<DailyDialogComponent>) { }
  ngOnInit() {
    // console.log(this.receivedNode.tableCounterName);
    // if(this.receivedNode.tableCounterName === undefined)
    // {
    //  this.isLeaf = false;
    // }
    // else
    // {
    //  this.isLeaf = true;
    // }
  }

}
