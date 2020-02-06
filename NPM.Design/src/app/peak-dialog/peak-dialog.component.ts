import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { SubsetCounter } from 'app/extraClasses/subsetCounter';
import { RequestOptions, Headers, Http } from '@angular/http';
import { Router } from '@angular/router';
import { Peak } from 'app/extraClasses/peak';

@Component({
  selector: 'app-peak-dialog',
  templateUrl: './peak-dialog.component.html',
  styleUrls: ['./peak-dialog.component.css']
})
export class PeakDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<PeakDialogComponent>) { }
  public receivedNode: Peak;
  private isLeaf: boolean = false;

  ngOnInit() {
    //console.log(this.receivedNode.tableCounterName);
    //if(this.receivedNode.tableCounterName === undefined)
    //{
    //  this.isLeaf = false;
    //}
    //else
    //{
    //  this.isLeaf = true;
    //}
  }

}
