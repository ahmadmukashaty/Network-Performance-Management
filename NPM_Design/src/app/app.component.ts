import { Component, Input } from '@angular/core';
import { SessionStorageService } from 'ng2-webstorage';
import { Router } from '@angular/router';
import { Location } from '@angular/common';



declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
 flag: boolean;

  constructor(
  private storage: SessionStorageService, private router: Router) {}


  ngOnInit() {
    console.log ('user is : ', this.storage.retrieve('currentUser') )
    // console.log('first flag', this.flag)
    if (this.storage.retrieve('currentUser')) {
      this.flag = true;
      if (!this.storage.retrieve('key')) {
        this.storage.store('key', true)
      window.location.reload();
      // this.router.navigate(['/app']);
      }
      console.log('user from sto: ', this.storage.retrieve('currentUser') )
      // console.log('flag is : ', this.flag)
      this.router.navigate(['/dashboard']);
    }
}
}

