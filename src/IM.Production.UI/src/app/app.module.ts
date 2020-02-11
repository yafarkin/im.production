import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatTabsModule } from '@angular/material/tabs';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';

import { TeamsComponent } from '../teams/teams.component';
import { ContractComponent } from '../contract/contract.component';
import { ContractsComponent } from '../contracts/contracts.component';
import { TeamsService } from '../services/teams.service';
import { TeamDetailsComponent } from '../team-details/team-details.component';
import { ContractsService } from '../services/contracts.service';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { OldAppComponent } from './old-app/app.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app/app.component';

@NgModule({
    declarations: [
        AppComponent,
        TeamsComponent,
        ContractComponent,
        ContractsComponent,
        OldAppComponent,
        TeamsComponent,
        LoginComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatButtonToggleModule,
        MatToolbarModule, MatDividerModule, MatListModule, MatGridListModule, MatCardModule,
        MatTabsModule,
        ContractsComponent,
        TeamsComponent,
        TeamDetailsComponent
    ],
    providers: [
        ContractsService,
        TeamsService,
        MatTableModule,
        HttpClientModule
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
