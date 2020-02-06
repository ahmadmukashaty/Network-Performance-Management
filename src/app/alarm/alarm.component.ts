import { Data, Router } from '@angular/router';
import { Alarm } from '../extraClasses/alarm';
import { Http } from '@angular/http';
import { Component, OnInit } from '@angular/core';
import { SessionStorageService } from 'ng2-webstorage';
declare var jquery: any;
declare var $: any;


@Component({
  selector: 'app-alarm',
  templateUrl: './alarm.component.html',
  styleUrls: ['./alarm.component.css']
})
export class AlarmComponent implements OnInit {
  loading: boolean;
  dat1: Date;
  dat: string;
  alarmdate: Date;
  unv: string;
  dateT: string;
  dateD: string;
  date1: Date;
  date: string;
  subsetsUrl: string;
data: Alarm[];
settings = {
  actions: false,
  columns: {
    AlarmId: {
      title: 'Alarm Id',
    },
    Action_Type: {
      title: 'Action Type',
      filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'MissingFile', title: 'MissingFile' },
            { value: 'MissingData', title: 'MissingData' },
          ],
        },
      },
    },
    Object_Type: {
      title: 'Object Type',
      filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'KPI', title: 'KPI' },
            { value: 'PEAK', title: 'PEAK' },
            { value: 'Subset', title: 'Subset' },
          ],
        },
      },
    },
    Granularity: {
      title: 'Granularity',
      filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Daily', title: 'Daily' },
            { value: 'Hourly', title: 'Hourly' },
            { value: 'Monthly', title: 'Monthly' },
          ],
        },
      },
    },
    Object_Name: {
      title: 'Object Name',
    /*  filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    UNV: {
      title: 'UNV',
     /* filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    First_Railse_Date: {
      title: 'First Raise Date',
     /* filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    Railse_Count: {
      title: 'Raise count(s)',
     /* filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    Clear_Date: {
      title: 'Clear Date',
     /* filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    Clear_Type: {
      title: 'Clear Type',
     /* filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Glenna Reichert', title: 'Glenna Reichert' },
            { value: 'Kurtis Weissnat', title: 'Kurtis Weissnat' },
            { value: 'Chelsey Dietrich', title: 'Chelsey Dietrich' },
          ],
        },
      },*/
    },
    Alarm_Type: {
      title: 'Alarm_Type',
      filter: {
        type: 'list',
        config: {
          selectText: 'Select...',
          list: [
            { value: 'Operation', title: 'Operation' },
            { value: 'U2000', title: 'U2000' },
          ],
        },
      },
    },
  },
};

constructor(private http: Http, private storage: SessionStorageService, private _router: Router,) {}

H6900() {
  this.unv = 'H69';
  this.getdata();
}


 GSN() {
   this.unv = 'GSN';
   this.getdata();
 }

 NSS() {
  this.unv = 'NSS';
  this.getdata();
}

getvalue() {
  return this.http.get(this.subsetsUrl)
  .map(sub => sub.json() as Alarm[]);
  }

getdata(): Alarm[] {
  this.loading = true;
 this.dat1 = this.alarmdate;
 this.dat = this.dat1.toString();
 this.dat = this.dat.replace('-', '/');
 this.dat = this.dat.replace('-', '/');
 console.log('this.dat', this.dat);
 this.subsetsUrl = 'http://seserv112/NPMWebAPI/api/Loading/GetAlarm?Alarm_Type=U2000&First_Railse_Date=' + this.dat + '&unv=' + this.unv
  this.getvalue().subscribe( response => {
    this.data = response;
    this.loading = false;
//   console.log('resp :' + this.resp);
});
   return this.data;
}



  ngOnInit() {
    if (!this.storage.retrieve('currentUser')){
      $('.alarmData').empty();
      this._router.navigate(['/login']);
  }
  }

}
