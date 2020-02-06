import { Component }                    from '@angular/core';
import { Router }                       from '@angular/router';

import { AlertService, UserService }   from '../services/index';

@Component({
    moduleId: module.id,
    templateUrl: 'register.component.html',
    styleUrls: ['./logIn.component.css']
})

export class RegisterComponent {
    msg: boolean;
    resp: string;
    model: any = {};
    loading = false;

    constructor(
        private router: Router,
        private usersService: UserService,
        private alertService: AlertService) { }

    register() {
        this.loading = true;
        this.usersService.create(this.model)
            .subscribe(
                data => {
                    this.resp = data.json() as string;
                    console.log('reg: ', this.resp);
                    if (this.resp === 'OK') {
                    this.alertService.success('Registration successful', true);
                    
                    this.router.navigate(['/login']);
                    } else {
                        this.msg = true;
                    this.router.navigate(['/register']);
                    }
                    this.loading = false;
                }); /*
            .catch(
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });*/
    }
}
