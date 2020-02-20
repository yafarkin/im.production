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
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule, MatSnackBarContainer } from '@angular/material';
import { Md5 } from 'ts-md5/dist/md5';

import { TeamsService } from '../services/teams.service';
import { ContractsService } from '../services/contracts.service';
import { AppComponent } from './app/app.component';
import { TeamComponent } from '../team/team.component';
import { FactoriesComponent } from '../factories/factories.component';
import { TeamService } from '../services/team.service';
import { TeamsComponent } from '../teams/teams.component';
import { ContractComponent } from '../contract/contract.component';
import { ContractsComponent } from '../contracts/contracts.component';
import { NewTeamComponent } from '../new-team/new-team.component';

// определение маршрутов
const appRoutes: Routes = [
    { path: 'teams', component: TeamsComponent },
    { path: 'team', component: TeamComponent },
    { path: 'contracts', component: ContractsComponent },
    { path: 'contracts/:id', component: ContractComponent },
    { path: 'new-team', component: NewTeamComponent }
];

@NgModule({
    declarations: [
        AppComponent,
        TeamsComponent,
        ContractComponent,
        ContractsComponent,
        TeamComponent,
        FactoriesComponent,
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
        MatInputModule,
        MatDialogModule,
        MatSnackBarModule
    ],
    providers: [
        ContractsService,
        TeamsService,
        TeamService,
        MatSnackBar,
        Md5
    ],
    bootstrap: [
        AppComponent
    ],
    entryComponents: [
        NewTeamComponent,
        MatSnackBarContainer
    ]
})
export class AppModule { }
