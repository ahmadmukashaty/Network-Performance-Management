import { Options } from 'ts-node/dist';
import { Logs } from '../extraClasses/logs';
import { Subscription } from 'rxjs/Rx';
import { Component, AfterViewInit } from '@angular/core';
import { SubsetsService } from '../services/getSubsets.service';
import { Subset } from '../extraClasses/subsets';
import { Observable } from 'rxjs/Observable';
import { Counter } from '../extraClasses/counters';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Router, ActivatedRoute, Params } from '@angular/router';
import 'rxjs/add/operator/switchMap';
import { ToAddSubset } from '../extraClasses/toAddSubset';
import { Dimention } from '../extraClasses/Dimention';
import { ANIMATION_TYPES } from 'ngx-loading';
import {LocalStorageService, SessionStorageService} from 'ng2-webstorage';
import { DashboardResponse } from 'app/extraClasses/DashboardResponse';
import { DimentionResponse } from 'app/extraClasses/DimentionResponse';
import { AddSubsetPost } from 'app/extraClasses/AddSubsetPost';
import { AddSubsetResponse } from 'app/extraClasses/AddSubsetResponse';
import { MatDialogConfig, MatDialogRef, MatDialog } from '@angular/material';
import { ResponseDialogComponent } from 'app/response-dialog/response-dialog.component';
import { UserInfo } from 'app/extraClasses/userInfo';
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-dashboard2',
  templateUrl: './dashboard2.component.html',
  styleUrls: ['./dashboard2.component.css'],
  
})
export class Dashboard2Component implements AfterViewInit {
    dims: Dimention[];
    DimexistBool: Dimention[];
    Dimexist: Dimention;
    userAlias: any;
    tempBool: boolean;
    user: string;
    unvName: any;
    setPathView: boolean;
    date: string;
    dateT: string;
    dateD: string;
    date1: Date;
    log: Logs;
    username: string;

    busy: Subscription;
    public loading = false;
    addRes: AddSubsetResponse;
    //showAddDim: boolean;
    dimsToAdd: Dimention[];
    dim: Dimention = new Dimention();
    existDim: Dimention;
    tempCounter: Counter = new Counter();
    countersToAdd: Counter[];
    exist: Counter;
    //showAddcounter: boolean;
    routString: string;
    subset1: Subset;
    unv1: string;
    subscribePath: any;
    rout: string;
    dispalyRout: string;
    pack: Subset;
    subset: Subset;
    resp1: any;
    sTemp: Subset;
    subsets: Subset[];
    tempSub: Subset;
    subsetRelease: string;
    subsetUnv: string;
    subsetId: string;
    counters: Counter[];
    value: string;
    resp: string[];
    showpath: boolean;
    disableAddSubsetBtn = false;
    subsetAdd: ToAddSubset = new ToAddSubset();
    postSubset: AddSubsetPost = new AddSubsetPost();
    showCounters: boolean = false;
    private map = new Map()
    private subsetsUrl = 'http://seserv112/CreationTree/api/getdimentions/?userAlias='

    subsetArray: Subset[][];
    release: string;

    universeName: string;

    constructor(private SubsetService: SubsetsService, private http: Http,
                private route: ActivatedRoute, private _router: Router,
                private storage: SessionStorageService, public dialog: MatDialog) { }

    //toggle cards sections
    toggleSubsetsCard() {
        $('.header-content').slideToggle();
    }
    toggleSubsetinfoCard() {
        $('.subsetInfo-content').slideToggle();
    }
    toggleCountersCard() {
        $('.Counters-content').slideToggle();
    }

    toggleDimentionsCard() {
        $('.Dimentions-content').slideToggle();
    }

    toggleAddSubsetCard() {
        $('.addSubset-content').slideToggle();
    }


    ShowCountersCard() {
        this.showCounters = true;
    }


    ngAfterViewInit(): void 
    {
        if (!this.storage.retrieve('currentUser')) 
        {
            $('.dashboardData').empty();
            this._router.navigate(['/login']);
        }
        this.universeName = 'H69';
        this.rout = this.InvertPath(this.route.snapshot.params['transPath']);
        this.dispalyRout = this.route.snapshot.params['transPath'];

        //you have got the path from BO page
        if (this.rout != null) 
        {
            this.setPathView = true;
            if (this.storage.retrieve('pathDim')) 
            {
                //array of dimentions
                this.dimsToAdd = this.storage.retrieve('pathDim');
                this.disableAddSubsetBtn = true;
            }

            if (this.storage.retrieve('pathCounter')) 
            {
                this.countersToAdd = this.storage.retrieve('pathCounter');
                this.disableAddSubsetBtn = true;
            }

            if (this.storage.retrieve('pathSubset')) {
                this.sTemp = this.storage.retrieve('pathSubset');
                this.subsetRelease = this.sTemp.release;
                this.subsetUnv = this.sTemp.unv;
                this.subsetId = this.sTemp.subsetID;
                this.counters = this.sTemp.measurements;
            }
            if (this.storage.retrieve('pathSubAdd')) {
                this.subsetAdd = this.storage.retrieve('pathSubAdd');
                this.fillDimPath(this.subsetAdd, this.rout);
            }
        }
        else 
        {
            this.dimsToAdd = [];
            this.countersToAdd = [];
            this.sTemp = null;
            this.disableAddSubsetBtn = false;
        }
        this.getSubsets();
    }

    CallSubsetsService(unvName: string) {
        this.setPathView = true;
        this.universeName = unvName;
        this.getSubsets();
    }

    getSubsets(): Subset[] 
    {

        this.loading = true;
        this.busy = this.SubsetService.getSubsets(this.universeName).subscribe(values => {
        this.subsets = values.data;
        this.loading = false});
        return this.subsets;
    }


    onSelect(sub: Subset) 
    {
        this.countersToAdd = [];
        this.dimsToAdd = [];
        this.resp = null;
        this.subsetRelease = sub.release;
        this.subsetAdd.release = sub.release;
        this.subsetUnv = sub.unv;
        this.subsetAdd.unv = sub.unv;
        this.subsetId = sub.subsetID;
        this.subsetAdd.subsetID = sub.subsetID;
        this.counters = sub.measurements;
        this.dims = sub.dimentions;
        // console.log('dim[0]', this.dims[0].name)
        this.subsetAdd.measurements = [];
        this.subsetAdd.dimentions = [];
        this.subset = sub;

        if (this.storage.retrieve(this.subsetId)) {
            return this.resp = this.storage.retrieve(this.subsetId);
        }
        return this.counters;
    }

    getDime(subsetId: string): Observable<DimentionResponse> 
    {
        this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
        console.log(this.subsetsUrl + this.userAlias + '&subsetID=' + subsetId);
        return this.http.get(this.subsetsUrl + this.userAlias + '&subsetID=' + subsetId)
                .map(sub => sub.json() as DimentionResponse);
    }

    showDime(): string[] 
    {
        if (this.storage.retrieve(this.subsetId)) {
            return this.resp = this.storage.retrieve(this.subsetId);
        }else {
        this.loading = true
        
        this.getDime(this.subsetId).subscribe( response => 
        {
            this.resp = response.data;
            this.loading = false;
            this.storage.store(this.subsetId, this.resp);
            if(response.success == -1)
            {
                this.openDialog2(response);
            }

        });
        return this.resp;
        }
    }


    addsubset() 
    {
        this.loading = true;
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const body = JSON.stringify(this.postSubset);

        return this.http.post('http://seserv112/CreationTree/api/addsubset/', body, options )
        .map(res => res.json() as AddSubsetResponse);


    }

    getUser(): string {
        this.username = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
        return this.username ;
      }

     showAddres(): AddSubsetResponse 
     {

        this.date1 =  new Date();
        this.dateD = this.date1.toLocaleDateString();
        this.dateT = this.date1.toLocaleTimeString();
        this.date = this.dateD + ' ' + this.dateT;
        this.postSubset.userAlias = this.getUser();
        this.postSubset.file_name = this.subsetId + '_' + this.date;
        this.postSubset.data = this.subsetAdd;
        
        this.addsubset().subscribe( response => 
        {
            this.addRes = response;
            this.loading = false;
            this.openDialog(this.addRes);
        });

        this.rout = null;
        
        return this.addRes;
     }


      

    getCounterCheck(counter: Counter): boolean 
    {

        if (this.map.has(counter.counterID)) {
            return this.map.get(counter.counterID);
        } else {
            this.map.set(counter.counterID, false);
            return this.map.get(counter.counterID);
        }
    }

    isToAdd(counter: Counter): boolean 
    {
          if (this.subsetAdd.measurements === []) {
              return false
          }
        this.exist = this.subsetAdd.measurements.filter(x => x.counterID === counter.counterID)[0];
        if (this.exist == null) {
            return false
        } else {
            return true
        }
    }

    isToAddDim(r: string) {
        if (this.dimsToAdd === []) {
            return false
        }
      this.Dimexist = this.dimsToAdd.filter(x => x.name === r)[0];
      if (this.Dimexist == null) {
          return false
      } else {
          return true
      }
    }

    isDimExist(r: string): boolean {
        console.log(this.dims);
        console.log('r is ', r);
        if (this.dims.length === 0) {
            return true;
        } else {
        this.DimexistBool = this.dims.filter(d => d.name === r);
        if (this.DimexistBool.length === 0 ) {
            return true;
        } else {
            return false;
        }
    }

    }

    selectCheck(counter: Counter)
    {

        this.exist = this.subsetAdd.measurements.filter(x => x.counterID === counter.counterID)[0];

        if ( this.exist == null) {
            counter.path = this.rout;
            this.subsetAdd.measurements.push(counter);
        }else {
            this.subsetAdd.measurements = this.subsetAdd.measurements.filter(x => x.counterID !== counter.counterID);
        }
        this.disableAddSubsetBtn = false;
        if (this.rout != null && (this.subsetAdd.dimentions.length + this.subsetAdd.measurements.length > 0)) {
            this.disableAddSubsetBtn = true;
        }
        this.countersToAdd = this.subsetAdd.measurements;
        this.setPathView = (this.subsetAdd.dimentions.length > 0 || this.subsetAdd.measurements.length > 0) ? true : false;

    }



    selectDimCheck(r: string)
    {
        
        this.existDim = this.subsetAdd.dimentions.filter(x => x.name === r)[0];

         if ( this.existDim == null) {
             this.dim.name = r;
             this.dim.path = this.rout;
            this.subsetAdd.dimentions.push(this.dim);
        }else {
            this.subsetAdd.dimentions = this.subsetAdd.dimentions.filter(x => x.name !== r);
        }

        this.dimsToAdd = this.subsetAdd.dimentions;

        this.disableAddSubsetBtn = false;
        if (this.rout != null && (this.subsetAdd.dimentions.length + this.subsetAdd.measurements.length > 0)) {
            this.disableAddSubsetBtn = true;
        }
        this.dim = new Dimention();

        this.setPathView = (this.subsetAdd.dimentions.length > 0 || this.subsetAdd.measurements.length > 0) ? true : false;
    }

    getPath() 
    {
        this.storage.store('pathCounter', this.countersToAdd);
        this.storage.store('pathSubset', this.subset);
        this.storage.store('pathDim', this.dimsToAdd);
        this.storage.store('pathSubAdd', this.subsetAdd);
        this._router.navigate(['/bo/' + this.universeName]);
    }

    fillDimPath(subToFill: ToAddSubset, arg1: string): any 
    {
        for ( let m = 0; m < subToFill.measurements.length;  m++) {
            subToFill.measurements[m].path = arg1;
        }
        for ( let d = 0; d < subToFill.dimentions.length;  d++) {
            subToFill.dimentions[d].path = arg1;
        }
    }

    InvertPath(path: string): string
    {
        if (path != null && path != 'undefined')
        {
            var tokens: string[] = path.split('/');
            tokens = tokens.reverse();
            var firstIteration:boolean = true;
            var newPath:string = '';
            for(let subPath of tokens)
            {
                if(firstIteration)
                {
                    newPath+= subPath;
                    firstIteration = false;
                }
                else
                {
                    newPath+= ('/'+subPath);
                }
            }
            return newPath;
        }
        return path;
    }

    openDialog(selectedNode: AddSubsetResponse) 
    {
       const config = new MatDialogConfig();
       const dialogRef: MatDialogRef<ResponseDialogComponent> = this.dialog.open(ResponseDialogComponent, config);
       dialogRef.componentInstance.receivedNode = selectedNode;
       dialogRef.componentInstance.file_name = this.postSubset.file_name;
       dialogRef.componentInstance.actionType = 'AddSubset';
       dialogRef.componentInstance.subsetID = this.subsetId;
     }

   openDialog2(selectedNode: DimentionResponse) 
   {
       const config = new MatDialogConfig();
       const dialogRef: MatDialogRef<ResponseDialogComponent> = this.dialog.open(ResponseDialogComponent, config);
       dialogRef.componentInstance.receivedNode = selectedNode;
       dialogRef.componentInstance.actionType = 'GetDimentions';
       dialogRef.componentInstance.subsetID = this.subsetId;
   }

}
