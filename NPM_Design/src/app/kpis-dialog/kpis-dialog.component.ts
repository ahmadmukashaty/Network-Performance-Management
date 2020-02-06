import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { SubsetCounter } from 'app/extraClasses/subsetCounter';
import { RequestOptions, Headers, Http } from '@angular/http';
import { Router } from '@angular/router';
import { Kpi } from 'app/extraClasses/kpi';

@Component({
  selector: 'app-kpis-dialog',
  templateUrl: './kpis-dialog.component.html',
  styleUrls: ['./kpis-dialog.component.css']
})
export class KpisDialogComponent implements OnInit {

  public receivedNode: Kpi;
  private isLeaf: boolean = false;

  constructor(public dialogRef: MatDialogRef<KpisDialogComponent>) { }

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
