import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';
import { SidebarModule } from './sidebar/sidebar.module';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule} from './shared/navbar/navbar.module';
import { FixedPluginModule} from './shared/fixedplugin/fixedplugin.module';
import { NguiMapModule} from '@ngui/map';
import { UserComponent } from './user/user.component';
import { TableComponent } from './table/table.component';
import { TypographyComponent } from './typography/typography.component';
import { IconsComponent } from './icons/icons.component';
import { MapsComponent } from './maps/maps.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { UpgradeComponent } from './upgrade/upgrade.component';
import { TreeComponent } from './bo/tree/tree.component';
import { TreeModule } from 'angular-tree-component/dist/angular-tree-component';
import { HttpModule } from '@angular/http';
import { MonitorComponent } from './monitor/monitor.component';
import { Dashboard2Component } from './dashboard2/dashboard2.component';
import { SubsetsService } from 'app/services/getSubsets.service';
import { LoadingModule, ANIMATION_TYPES } from 'ngx-loading';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { BoComponent } from './bo/bo.component';
import { FilterPipe} from './filter.pipe';
import { Ng2Webstorage } from 'ng2-webstorage';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MatDialogModule, MatCardModule, MatButtonModule, MatFormFieldModule, MatCheckboxModule  } from '@angular/material';
import { DialogComponent } from './dialog/dialog.component';
import { PeakfilterPipe } from './peakfilter.pipe';
import { KpipipePipe } from './kpipipe.pipe';
import { DailypipePipe } from './dailypipe.pipe';
import { DailyDialogComponent } from './daily-dialog/daily-dialog.component';
import { KpisDialogComponent } from './kpis-dialog/kpis-dialog.component';
import { PeakDialogComponent } from './peak-dialog/peak-dialog.component';
import { AlarmComponent } from './alarm/alarm.component';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { CounterFilterPipe } from 'app/counterfilter.pipe';
import { ResponseDialogComponent } from './response-dialog/response-dialog.component';
import { LoginComponent } from './auth/index';
import { RegisterComponent } from './auth/index';
import {UserService, UsersService, AlertService, AuthenticationService  } from './services/index';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ActionsComponent } from './actions/actions.component';
import { ActionFilterPipe } from 'app/actionsFilter.pipe';


@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    TableComponent,
    TypographyComponent,
    IconsComponent,
    MapsComponent,
    NotificationsComponent,
    UpgradeComponent,
    TreeComponent,
    MonitorComponent,
    Dashboard2Component,
    BoComponent,
    FilterPipe,
    DialogComponent,
    PeakfilterPipe,
    KpipipePipe,
    DailypipePipe,
    DailyDialogComponent,
    KpisDialogComponent,
    PeakDialogComponent,
    AlarmComponent,
    CounterFilterPipe,
    ActionFilterPipe,
    ResponseDialogComponent,
	    LoginComponent,
    RegisterComponent,
    ActionsComponent
    
],
  imports: [
    BrowserModule,
    RouterModule.forRoot(AppRoutes, {useHash: true}),
    SidebarModule,
    NavbarModule,
    FooterModule,
    FixedPluginModule,
    TreeModule,
    HttpModule,
    LoadingModule,
    FormsModule,
    ChartsModule,
    Ng2Webstorage,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatDialogModule,
    MatCheckboxModule,
    Ng2SmartTableModule,
    NguiMapModule.forRoot({apiUrl: 'https://maps.google.com/maps/api/js?key=AIzaSyBr-tgUtpm8cyjYVQDrjs8YpZH7zBNWPuY'})

  ],
  providers: [SubsetsService , AlertService,
    AuthenticationService, UsersService, UserService, {provide: LocationStrategy, useClass: HashLocationStrategy}],
  entryComponents: [ DialogComponent, DailyDialogComponent, KpisDialogComponent, PeakDialogComponent, ResponseDialogComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
