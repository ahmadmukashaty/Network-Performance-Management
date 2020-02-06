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
    showAddDim: boolean;
    dimsToAdd: Dimention[];
    dim: Dimention = new Dimention();
    existDim: Dimention;
    tempCounter: Counter = new Counter();
    countersToAdd: Counter[];
    exist: Counter;
    showAddcounter: boolean;
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
    disable1 = false;
    subsetAdd: ToAddSubset = new ToAddSubset();
    postSubset: AddSubsetPost = new AddSubsetPost();
    showCounters: boolean = false;
    private map = new Map()
    private subsetsUrl = 'http://seserv112/CreationTree/api/getdimentions/?userAlias='

    subsetArray: Subset[][];
    release: string;

    universeName: string;

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

          // transPath = this.route.routerState.root.queryParams['transPath'];
          // transPath = this.route.snapshot.params['transPath'];

       // In a real app: dispatch action to load the details here.



         constructor(private SubsetService: SubsetsService, private http: Http,
                                     private route: ActivatedRoute, private _router: Router,
                                     private storage: SessionStorageService, public dialog: MatDialog,
                                      ) { }

     // http: Http


          ngAfterViewInit(): void {
            if (!this.storage.retrieve('currentUser')) {
                $('.dashboardData').empty();
                this._router.navigate(['/login']);
            }
             this.universeName = 'H69';
              // console.log(this.universeName);
            this.rout = this.InvertPath(this.route.snapshot.params['transPath']);
            this.dispalyRout = this.route.snapshot.params['transPath'];
             // console.log(this.rout);
             if (this.rout != null) {
                this.setPathView = true;
                if (this.storage.retrieve('pathDim')) {
                    this.dimsToAdd = this.storage.retrieve('pathDim');
                    this.showAddDim = true;
                    this.disable1 = true;
                }

                if (this.storage.retrieve('pathCounter')) {
                    this.countersToAdd = this.storage.retrieve('pathCounter');
                    this.showAddcounter = true;
                    this.disable1 = true;
                }
                if (this.storage.retrieve('pathSubset')) {
                    this.sTemp = this.storage.retrieve('pathSubset');
                    this.subsetRelease = this.sTemp.release;
                    this.subsetUnv = this.sTemp.unv;
                    this.subsetId = this.sTemp.subsetID;
                }
                if (this.storage.retrieve('pathSubAdd')) {
                    this.subsetAdd = this.storage.retrieve('pathSubAdd');
                    this.fillDimPath (this.subsetAdd, this.rout);
                }
             }else {
                this.dimsToAdd = [];
                this.countersToAdd = [];
                this.sTemp = null;
                this.disable1 = false;
             }

              console.log('subset to add: ', this.subsetAdd);
              this.getSubsets();
              // this.showWinUser();
            }

            CallSubsetsService(unvName: string) {
                this.setPathView = true;
                this.universeName = unvName;
                // console.log(this.universeName);
                this.getSubsets();
            }

          getSubsets(): Subset[] {

                this.loading = true;
                this.busy = this.SubsetService.getSubsets(this.universeName).subscribe(values => {
                   // console.log("subser service");
                 console.log('this.values', values);
                this.subsets = values.data;
                /*for (let i=0; i<this.key; i++){
                    this.subsetArray[i]==this.subsets[i];
                }*/
               // console.log(this.subsets);
                this.loading = false});
               return this.subsets;
              // this.nodes = this.NodeService.getNodes();
            }


            onSelect(sub: Subset) {
                this.countersToAdd = [];
                this.dimsToAdd = [];
                this.resp = null;
                // console.log( 'trans:' + this.rout);
                this.subsetRelease = sub.release;
                this.subsetAdd.release = sub.release;
                this.subsetUnv = sub.unv;
                this.subsetAdd.unv = sub.unv;
                this.subsetId = sub.subsetID;
                this.subsetAdd.subsetID = sub.subsetID;
                this.counters = sub.measurements;
                this.subsetAdd.measurements = [];
                this.subsetAdd.dimentions = [];
                this.subset = sub;

                if (this.storage.retrieve(this.subsetId)) {
                    return this.resp = this.storage.retrieve(this.subsetId);
                }
                return this.counters;
            }

            getDime(subsetId: string): Observable<DimentionResponse> {


                this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
                return this.http.get(this.subsetsUrl + this.userAlias + '&subsetID=' + subsetId)
                     .map(sub => sub.json() as DimentionResponse);
            }

            showDime(): string[] {
                if (this.storage.retrieve(this.subsetId)) {
                    return this.resp = this.storage.retrieve(this.subsetId);
                }else {
                this.loading = true
                console.log(this.subsetsUrl + this.subsetId);
                this.getDime(this.subsetId).subscribe( response => {
                        this.resp = response.data;
                       console.log('resp :' + this.resp);
                        this.loading = false
                    this.storage.store(this.subsetId, this.resp)
                });
                return this.resp;
                }
            }


    addsubset() {
            // this.subset.dimentions = this.resp;

            // this.pack = this.subset;
            // console.log( "pack : "+ this.subset);
            console.log('loading1 is: ', this.loading)
            this.loading = true;
            console.log('loading2 is: ', this.loading)
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

     showAddres(): AddSubsetResponse {

        this.date1 =  new Date();
        this.dateD = this.date1.toLocaleDateString();
        this.dateT = this.date1.toLocaleTimeString();
        this.date = this.dateD + ' ' + this.dateT;
        // this.loading = true
        this.postSubset.userAlias = this.getUser();
        this.postSubset.file_name = this.subsetId + '_' + this.date;
        this.postSubset.data = this.subsetAdd;
        console.log('post subset info :' + JSON.stringify(this.postSubset));
        
        this.addsubset().subscribe( response => {
            this.addRes = response;
            // console.log(this.addRes);
            this.loading = false;
            this.openDialog(this.addRes);
        });
        this.rout = null;
        
        return this.addRes;
     }

     openDialog(selectedNode: AddSubsetResponse) {
        const config = new MatDialogConfig();
        const dialogRef: MatDialogRef<ResponseDialogComponent> = this.dialog.open(ResponseDialogComponent, config);
        dialogRef.componentInstance.receivedNode = selectedNode;
        dialogRef.componentInstance.file_name = this.postSubset.file_name;
        dialogRef.componentInstance.actionType = 'AddSubset';
      }

      getCounterCheck(counter: Counter): boolean {

        if (this.map.has(counter.counterID)) {
            return this.map.get(counter.counterID);
        } else {
            this.map.set(counter.counterID, false);
            return this.map.get(counter.counterID);
        }
      }

      isToAdd(counter: Counter): boolean {
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

selectCheck(counter: Counter)  {
        /*this.tempBool = this.map.get(counter.counterID);
        this.tempBool = !this.tempBool;
        this.map.delete(counter.counterID);
        this.map.set(counter.counterID, this.tempBool);
        this.getCounterCheck(counter);*/

        this.setPathView = true;
         this.showAddcounter = true;

         this.exist = this.subsetAdd.measurements.filter(x => x.counterID === counter.counterID)[0];

         if ( this.exist == null) {
            counter.path = this.rout;
            console.log(this.rout);
            this.subsetAdd.measurements.push(counter);
        }else {
            this.subsetAdd.measurements = this.subsetAdd.measurements.filter(x => x.counterID !== counter.counterID);
        }
        this.disable1 = false;
        if (this.rout != null && (this.subsetAdd.dimentions.length + this.subsetAdd.measurements.length > 0)) {
            this.disable1 = true;
        }
        this.countersToAdd = this.subsetAdd.measurements;

    }



 selectDimCheck(r: string) {
     this.setPathView = true;
     this.showAddDim = true;
        this.existDim = this.subsetAdd.dimentions.filter(x => x.name === r)[0];

         if ( this.existDim == null) {
             this.dim.name = r;
             this.dim.path = this.rout;
            this.subsetAdd.dimentions.push(this.dim);
        }else {
            this.subsetAdd.dimentions = this.subsetAdd.dimentions.filter(x => x.name !== r);
        }

        this.dimsToAdd = this.subsetAdd.dimentions;

        this.disable1 = false;
        if (this.rout != null && (this.subsetAdd.dimentions.length + this.subsetAdd.measurements.length > 0)) {
            this.disable1 = true;
        }
        this.dim = new Dimention();
 }

 getPath() {
    this.storage.store('pathCounter', this.countersToAdd)
    this.storage.store('pathSubset', this.subset);
    this.storage.store('pathDim', this.dimsToAdd);
    this.storage.store('pathSubAdd', this.subsetAdd);
     this._router.navigate(['/bo/' + this.universeName]);
 }

 fillDimPath(subToFill: ToAddSubset, arg1: string): any {
    for ( let m = 0; m < subToFill.measurements.length;  m++) {
        subToFill.measurements[m].path = arg1;
    }

    for ( let d = 0; d < subToFill.dimentions.length;  d++) {
        subToFill.measurements[d].path = arg1;
    }
}

InvertPath(path:string):string
{
    console.log(path);
    if(path != null && path != "undefined")
    {
        var tokens:string[] = path.split('/');
        tokens = tokens.reverse();
        console.log(tokens.length);
        var firstIteration:boolean = true;
        var newPath:string = "";
        for(let subPath of tokens)
        {
            if(firstIteration)
            {
                newPath+= subPath;
                firstIteration = false;
            }
            else
            {
                newPath+= ("/"+subPath);
            }
        }
        console.log(newPath);
        return newPath;
    }
    return path;
}

 /*getWinUser(): Observable<string> {

     const headers1 = new Headers({'Content-Type': 'application/json'});
     headers1.append('Content-Type', 'application/x-www-form-urlencoded');
     const options1 = new RequestOptions({ headers: headers1 });
     const data = 'grant_type=password&username=RabeeA&password=sosyRabie.-2191';
     // const body = null;
    return this.http.post('http://seserv112/winauth/api/winuser', data, options1)
         .map(sub => sub.toString());
}

showWinUser(): string {

    this.getWinUser().subscribe( response => {
            this.user = response;
        //   console.log('resp :' + this.resp);
            console.log('user is : ' + this.user)
        });
           return this.user;

}*/

}
