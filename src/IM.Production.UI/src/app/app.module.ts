import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';

import { AppComponent } from './app.component';
import { TeamsComponent } from '../teams/teams.component';
import { ContractComponent } from '../contract/contract.component';
import { ContractsComponent } from '../contracts/contracts.component';
@NgModule({
    declarations: [
        AppComponent,
        TeamsComponent,
        ContractComponent,
        ContractsComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        FormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatButtonToggleModule,
        MatToolbarModule, MatDividerModule, MatListModule, MatGridListModule
    ],
    providers: [],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
