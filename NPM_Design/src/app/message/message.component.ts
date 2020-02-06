import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  public receivedMessage: string;

  constructor(public dialogRef: MatDialogRef<MessageComponent>) { }

  ngOnInit() {
  }

}
