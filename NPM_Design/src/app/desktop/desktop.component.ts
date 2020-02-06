import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionStorageService } from 'ng2-webstorage';
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-desktop',
  templateUrl: './desktop.component.html',
  styleUrls: ['./desktop.component.css']
})
export class DesktopComponent implements OnInit {

  constructor(private storage: SessionStorageService,private _router: Router) { }

  ngOnInit() {
    if (!this.storage.retrieve('currentUser')) {
      $('.desktopData').empty();
      this._router.navigate(['/login']);
    }
  }

}
