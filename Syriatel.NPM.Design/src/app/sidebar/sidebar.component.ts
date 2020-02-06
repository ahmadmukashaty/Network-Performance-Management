import { UserInfo } from '../extraClasses/userInfo';
import { Component, OnInit } from '@angular/core';
import { SessionStorageService } from 'ng2-webstorage';

declare var $:any;

export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    role1: string;
    role2: string;
}

export let ROUTES: RouteInfo[] = [
    { path: 'dashboard', title: 'Dashboard',  icon: 'ti-panel', role1: 'administrator', role2: 'regular' },
    { path: 'bo/H69', title: 'Business Object',  icon:'ti-server', role1: 'administrator', role2: 'regular' },
    { path: 'actions', title: 'Actions',  icon:'ti-shift-right', role1: 'administrator', role2: 'regular' },
    //{ path: 'monitor', title: 'Monitor',  icon:'ti-bar-chart-alt', role1: 'administrator', role2: '' },
   // { path: 'user', title: 'User Profile',  icon:'ti-user', class: '' },
    //{ path: 'notifications', title: 'Notifications',  icon:'ti-bell', class: '' },
    //{ path: 'alarm', title: 'Alarms',  icon:'ti-bell', role1: 'administrator', role2: ''},
   // { path: 'typography', title: 'Typography',  icon:'ti-text', class: '' },
   // { path: 'icons', title: 'Icons',  icon:'ti-pencil-alt2', class: '' },
   // { path: 'maps', title: 'Maps',  icon:'ti-map', class: '' },
   // { path: 'notifications', title: 'Notifications',  icon:'ti-bell', class: '' },
    
   // { path: 'dashboard2', title: 'Dashboard',  icon:'ti-server', class: '' },
    
   // { path: 'upgrade', title: 'Upgrade to PRO',  icon:'ti-export', class: 'active-pro' },
];

@Component({
    moduleId: module.id,
    selector: 'sidebar-cmp',
    templateUrl: 'sidebar.component.html',
})

export class SidebarComponent implements OnInit {
    userAlias: string;
    user: UserInfo;
    public menuItems: any[];
    constructor (private storage: SessionStorageService) {}
    
    ngOnInit() {
        if (this.storage.retrieve('currentUser')) {
            this.userAlias = (<UserInfo>this.storage.retrieve('currentUser')).userAlias;
        }
        this.menuItems = ROUTES.filter(menuItem => menuItem);
        this.auth();
    }
    isNotMobileMenu() {
        if ($(window).width() > 991) {
            return false;
        }
        return true;
    }

    auth(): void {
        this.user = this.storage.retrieve('currentUser')
        if (this.user.userRole === 'administrator') {
            this.menuItems.push({ path: 'monitor', title: 'Monitor',  icon: 'ti-bar-chart-alt', role1: 'administrator', role2: '' },
            { path: 'alarm', title: 'Alarms',  icon: 'ti-bell', role1: 'administrator', role2: ''})

    }

}}
