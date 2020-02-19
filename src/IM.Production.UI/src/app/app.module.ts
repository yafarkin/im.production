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
import { AppComponent } from './app/app.component';
import { TeamComponent } from '../team/team.component';
import { FactoriesComponent } from '../factories/factories.component';
import { TeamService } from '../services/team.service';

// определение маршрутов
const appRoutes: Routes = [
    { path: 'teams', component: TeamsComponent },
    { path: 'teamDetails', component: TeamDetailsComponent },
    { path: 'team', component: TeamComponent },
    { path: 'contracts', component: ContractsComponent },
    { path: 'contracts/:id', component: ContractComponent }
];

@NgModule({
   declarations: [
      AppComponent,
      TeamDetailsComponent,
      TeamsComponent,
      ContractComponent,
      ContractsComponent,
      TeamComponent,
      FactoriesComponent
   ],
   imports: [
      CommonModule,
      BrowserModule,
      RouterModule.forRoot(appRoutes),
      FormsModule,
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
      MatTabsModule
   ],
   providers: [
      ContractsService,
      TeamsService,
      TeamService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
