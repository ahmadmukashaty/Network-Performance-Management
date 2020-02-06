import { Daily } from '../extraClasses/daily';
import { Kpi } from '../extraClasses/kpi';
import { Peak } from '../extraClasses/peak';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { NodesService } from '../services/getNodes.service';
import { Node } from '../extraClasses/nodes';
import { Routes, RouterModule, Router } from '@angular/router';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Loading } from '../extraClasses/loading';
import { ChartsModule } from 'ng2-charts';
import { ANIMATION_TYPES } from 'ngx-loading';
import { MatDialogConfig, MatDialogRef, MatDialog } from '@angular/material';
import { DailyDialogComponent } from 'app/daily-dialog/daily-dialog.component';
import { PeakDialogComponent } from 'app/peak-dialog/peak-dialog.component';
import { KpisDialogComponent } from 'app/kpis-dialog/kpis-dialog.component';
import { SessionStorageService } from 'ng2-webstorage';
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-monitor',
  templateUrl: './monitor.component.html',
  styleUrls: ['./monitor.component.css']
})
export class MonitorComponent implements OnInit{

  monitor: boolean;
  loading: boolean;
  dailyNote: string;
  dailySucc: string;
  dailyResAlarm: string;
  dailyExpDate: string;
  dailyUnv: string;
  dailyId: string;
  kpiNote: string;
  kpiSucc: string;
  kpiResAlarm: string;
  kpiExpDate: string;
  kpiUnv: string;
  kpiId: string;
  peakNote: string;
  peakSucc: string;
  peakResAlarm: string;
  peakExpDate: string;
  peakUnv: string;
  peakId: string;
  dateCard: string;
  daily: Daily[];
  kpi: Kpi[];
  peak: Peak[];

    barChartLabels: string[];
    barChartType: string;
    barChartLegend: boolean;
    barChartData: any;
    barChartOptions: { scaleShowVerticalLines: boolean; responsive: boolean; };
    loadTemp: Loading[];
    dFrom: Date;
    Dat: string;
    load: Loading[];
    subsetsUrl: string;
    response: boolean;
    Estimated = 'Estimated count';
    Actual = 'Actual count';
    Missing = 'Missing count';
    tempDataEs: Array<number> = [];
    tempDataAc: Array<number> = [];
    tempDataMi: Array<number> = [];
    temp: Array<number> = [5];
    dateFrom: Date;
    dateToShow: Date;
    testDate = new Date();
    dateString: string;
    
    // tslint:disable-next-line:no-inferrable-types
    unv: string;
    dayList: Array<string> = ['00', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15',
                               '16', '17', '18', '19', '20', '21', '22', '23'];
    


  constructor(private _router: Router, private http: Http, public Dailydialog: MatDialog,
               public KPIsdialog: MatDialog, public Peakdialog: MatDialog,private storage: SessionStorageService  ) { }

   // public lineChartData:Array<any> = new Array<any>();

   ngOnInit(): void {
    if (!this.storage.retrieve('currentUser')){
      $('.monitorData').empty();
      this._router.navigate(['/login']);
    }
  }

   H6900() {
    this.monitor = true;
    this.unv = 'H69';
    this.showData();
  }


   GSN() {
     this.monitor = null;
     this.unv = 'GSN';
     this.showData();
   }

   NSS() {
    this.monitor = null;
    this.unv = 'NSS';
    this.showData();
  }

   getData(Dat: string, unv: string): Observable<Loading[]> {

   this.subsetsUrl = 'http://seserv112/NPMWebAPI/api/Loading/GetLoadingLog?Date=' + this.Dat + '&unv=' + this.unv;  // URL to web api
    return this.http.get(this.subsetsUrl)
                 .map(sub => sub.json() as Loading[])

   }

   getValues(Dat: string, unv: string): Loading[] {

    this.tempDataEs = [];
    this.tempDataAc = [];
    this.tempDataMi = [];

     this.getData(Dat, unv).subscribe( values => {
                  this.load = values;

if ( this.load.length !== 0) {
         this.tempDataEs.push(this.load[0].EstimatedCount);
         this.tempDataAc.push(this.load[0].LoadingCount);
         this.tempDataMi.push(this.load[0].MissingCount);
          } else {
            this.tempDataEs.push(0);
         this.tempDataAc.push(0);
         this.tempDataMi.push(0);
          }

});
// console.log('this.load : ' , this.load)
                  return this.load;
   }

   showData() {
     this.loading = true;
    this.dateToShow = this.dateFrom;
    this.testDate = this.dateFrom;
    this.dateString = this.testDate.toString();
    this.dateCard = this.testDate.toString();
    this.dateString = this.dateString.replace('-', '');
    this.dateString = this.dateString.replace('-', '');
    this.dateCard = this.dateCard.replace('-', '/');
    this.dateCard = this.dateCard.replace('-', '/');
    console.log('date string : ', this.dateCard);
      for (let i = 0; i < 24; i++) {
        this.Dat = this.dateString + this.dayList[i];
        this.getValues(this.Dat, this.unv);

        }

        this.setData();
        this.getPeakValues(this.dateCard);
        this.getKpiValues(this.dateCard);
        this.getDailyValues(this.dateCard);

        this.loading = false;
   }


  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }





setData() {

    this.barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true

  };
  this.barChartLabels = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11',
                        '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23'];
  this.barChartType = 'bar';
  this.barChartLegend = true;

  this.barChartData = [
    {data: this.tempDataEs, label: this.Estimated},
    {data: this.tempDataAc, label: this.Actual},
    {data: this.tempDataMi, label: this.Missing}
  ];

}


getPeakData(Dat: string): Observable<Peak[]> {

     this.subsetsUrl = 'http://seserv112/NPMWebAPI/api/Loading/GetPEAKLog?Execute_date=' + Dat;  // URL to web api
      return this.http.get(this.subsetsUrl)
                   .map(sub => sub.json() as Peak[])

     }

getPeakValues (Dat: string): Peak[] {
  this.getPeakData(Dat).subscribe( values => {
    this.peak = values;
    console.log(this.peak);
        })

    return this.peak;
  }




getKpiData(Dat: string): Observable<Kpi[]> {

     this.subsetsUrl = 'http://seserv112/NPMWebAPI/api/Loading/GetKPILog?Execute_date=' + Dat;  // URL to web api
      return this.http.get(this.subsetsUrl)
                   .map(sub => sub.json() as Kpi[])

     }

getKpiValues (Dat: string): Kpi[] {
  this.getKpiData(Dat).subscribe( values => {
    this.kpi = values;
    console.log(this.kpi);
        })

    return this.kpi;
 }




 getDailyData(Dat: string): Observable<Daily[]> {

       this.subsetsUrl = 'http://seserv112/NPMWebAPI/api/Loading/GetDailyLog?Execute_date=' + Dat;  // URL to web api
        return this.http.get(this.subsetsUrl)
                     .map(sub => sub.json() as Daily[])

       }

  getDailyValues (Dat: string): Daily[] {
    this.getDailyData(Dat).subscribe( values => {
      this.daily = values;
      console.log(this.daily);
      this.loading = false;
          })
      return this.daily;
   }

   onSelectPeak(p) {
     const PeakSelected = new Peak();
     PeakSelected.PeakID = p.PeakID;
     PeakSelected.Unv = p.Unv;
     PeakSelected.ExpectedDate = p.ExpectedDate;
     PeakSelected.ReasonAlarmID = p.ReasonAlarmID;
     PeakSelected.Success = p.Success;
     PeakSelected.Note = p.Note;
     // this.peakId = p.PeakID;
     // this.peakUnv = p.Unv;
     // this.peakExpDate = p.ExpectedDate;
     // this.peakResAlarm = p.ReasonAlarmID;
     // this.peakSucc = p.Success;
     // this.peakNote = p.Note;
     this.openPeaksDialog(PeakSelected);
   }


   onSelectKpi(k) {
    const kpiSelected = new Kpi();
    kpiSelected.KpiID = k.KpiID;
    kpiSelected.Unv = k.Unv;
    kpiSelected.ExpectedDate = k.ExpectedDate;
    kpiSelected.ReasonAlarmID = k.ReasonAlarmID;
    kpiSelected.Success = k.Success;
    kpiSelected.Note = k.Note;
    // this.kpiId = k.KpiID;
    // this.kpiUnv = k.Unv;
    // this.kpiExpDate = k.ExpectedDate;
    // this.kpiResAlarm = k.ReasonAlarmID;
    // this.kpiSucc = k.Success;
    // this.kpiNote = k.Note;
    this.openKPIsDialog(kpiSelected);
  }


  onSelectDaily(d) {
    const dailySelected = new Daily();
    dailySelected.SubsetID = d.SubsetID;
    dailySelected.Unv = d.Unv;
    dailySelected.ExpectedDate = d.ExpectedDate;
    dailySelected.ReasonAlarmID = d.ReasonAlarmID;
    dailySelected.Success = d.Success;
    dailySelected.Note = d.Note;
    this.openDailyDialog(dailySelected);
    // this.dailyId = d.SubsetID;
    // this.dailyUnv = d.Unv;
    // this.dailyExpDate = d.ExpectedDate;
    // this.dailyResAlarm = d.ReasonAlarmID;
    // this.dailySucc = d.Success;
    // this.dailyNote = d.Note;
  }

  openDailyDialog(selectedNode: Daily) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<DailyDialogComponent> = this.Dailydialog.open(DailyDialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
  }

  openKPIsDialog(selectedNode: Kpi) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<KpisDialogComponent> = this.KPIsdialog.open(KpisDialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
  }

  openPeaksDialog(selectedNode: Peak) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<PeakDialogComponent> = this.Peakdialog.open(PeakDialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
  }

  togglePeak() {
      $('.peak-content').slideToggle();
  }

  toggleKPI() {
      $('.kpi-content').slideToggle();
  }

  toggleDaily() {
      $('.daily-content').slideToggle();
  }


}
