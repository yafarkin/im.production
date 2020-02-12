import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Routes, RouterModule } from '@angular/router';

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
import { MatInputModule } from '@angular/material/input';

import { TeamsComponent } from '../teams/teams.component';
import { ContractComponent } from '../contract/contract.component';
import { ContractsComponent } from '../contracts/contracts.component';
import { TeamsService } from '../services/teams.service';
import { TeamDetailsComponent } from '../team-details/team-details.component';
import { ContractsService } from '../services/contracts.service';
import { MatTableModule } from '@angular/material/table';
import { AppComponent } from './app/app.component';
import { NewTeamComponent } from '../new-team/new-team.component';

// определение маршрутов
const appRoutes: Routes = [
    { path: '', component: TeamsComponent },
    { path: 'teamDetails', component: TeamDetailsComponent },
    { path: 'contracts', component: ContractsComponent },
    { path: 'contracts/:id', component: ContractComponent },
    { path: 'new-team', component: NewTeamComponent }
];

@NgModule({
    declarations: [
        AppComponent,
        TeamDetailsComponent,
        TeamsComponent,
        ContractComponent,
        ContractsComponent,
        NewTeamComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        RouterModule.forRoot(appRoutes),
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatToolbarModule,
        MatDividerModule,
        MatListModule,
        MatGridListModule,
        MatCardModule,
        MatTabsModule,
        MatInputModule
    ],
    providers: [
        ContractsService,
        TeamsService
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
