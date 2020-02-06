import { Component, OnInit }                    from '@angular/core';
import { Router, ActivatedRoute }               from '@angular/router';

import { AlertService, AuthenticationService }  from '../services/index';
import {LocalStorageService, SessionStorageService} from 'ng2-webstorage';
import { UserInfo } from 'app/extraClasses/userInfo';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./logIn.component.css']

})

export class LoginComponent implements OnInit {
    msg: boolean;
    user: UserInfo = new UserInfo();
    resp: string;
    model: any = {};
    loading = false;
    returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private storage: SessionStorageService) {
    }

    ngOnInit() {        
        //window.location.reload();
        // reset login status
        this.authenticationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.params['returnUrl'] || '/';
    }

    login() {
        this.loading = true;
        this.authenticationService.login(this.model.username, this.model.password)
        .subscribe(res => { this.resp = res.json() as string;
                                            this.loading = false;
                                            console.log('res: ', this.resp);
                                            if (this.resp === 'administrator' || this.resp === 'regular') {
                                                this.user.userAlias = this.model.username;
                                                this.user.userRole = this.resp;
                                                this.storage.store('currentUser', this.user);
                                                // alert('done successfuly')
                                                window.location.reload();

                                            } else if ( this.resp === 'Unauthorized') {
                                                // alert('you are not authorized');
                                                this.msg = true
                                            }

        });
    }
}
