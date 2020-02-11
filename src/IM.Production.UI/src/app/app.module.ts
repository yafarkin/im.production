import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Routes, RouterModule } from '@angular/router';

import { MatTabsModule } from '@angular/material/tabs';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ContractsComponent } from '../contracts/contracts.component';
import { TeamsComponent } from '../teams/teams.component';
import { TeamsService } from '../services/teams.service';
import { TeamDetailsComponent } from '../team-details/team-details.component';
import { MatTableModule } from '@angular/material/table';
import { AppComponent } from './app/app.component';

// определение маршрутов
const appRoutes: Routes = [
    { path: '', component: TeamsComponent },
    { path: 'teamDetails', component: TeamDetailsComponent }
];

@NgModule({
    declarations: [
        AppComponent,
        ContractsComponent,
        TeamsComponent,
        TeamDetailsComponent
    ],
    imports: [
        CommonModule, BrowserModule, FormsModule,
        MatTableModule, MatPaginatorModule, MatSortModule,
        MatButtonModule, MatButtonToggleModule, MatToolbarModule,
        MatTabsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        BrowserModule, RouterModule.forRoot(appRoutes)
    ],
    providers: [
        TeamsService
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
