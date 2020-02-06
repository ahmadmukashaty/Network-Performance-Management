import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { SessionStorageService } from 'ng2-webstorage';
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-bo',
  templateUrl: './bo.component.html',
  styleUrls: ['./bo.component.css']
})
export class BoComponent implements OnInit {

  constructor(private route: ActivatedRoute, private _router: Router,private storage: SessionStorageService) { }

  ngOnInit() {
    if (!this.storage.retrieve('currentUser')){
      $('.boData').empty();
      this._router.navigate(['/login']);
  }
  }

}
