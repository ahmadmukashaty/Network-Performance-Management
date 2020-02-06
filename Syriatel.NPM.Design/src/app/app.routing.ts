import { AppComponent } from './app.component';
import { AlarmComponent } from './alarm/alarm.component';
import { Routes } from '@angular/router';

import { UserComponent }   from './user/user.component';
import { TableComponent }   from './table/table.component';
import { TypographyComponent }   from './typography/typography.component';
import { IconsComponent }   from './icons/icons.component';
import { MapsComponent }   from './maps/maps.component';
import { NotificationsComponent }   from './notifications/notifications.component';
import { UpgradeComponent } from './upgrade/upgrade.component';
import { TreeComponent } from './bo/tree/tree.component';
import { BoComponent } from './bo/bo.component';
import { Dashboard2Component } from './dashboard2/dashboard2.component';
import { MonitorComponent } from './monitor/monitor.component';
import { LoginComponent } from './auth/index';
import { RegisterComponent } from './auth/index';
import { ActionsComponent } from 'app/actions/actions.component';

export const AppRoutes: Routes = [
    {
        path: 'bo',
        component: BoComponent,
        children: [
            { path: ':unvName', component: TreeComponent }
        ]
    },
    {
        path: 'app',
        component: AppComponent,
    },

    {
        path: 'dashboard',
        component: Dashboard2Component
    },
    {
        path: 'monitor',
        component: MonitorComponent
    },
    {
        path: 'actions',
        component: ActionsComponent
    },
    {
        path: 'alarm',
        component: AlarmComponent
    },
    // {
    //     path: 'icons',
    //     component: IconsComponent
    // },
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full',
    },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent }
]
