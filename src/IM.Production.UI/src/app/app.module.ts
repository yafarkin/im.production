import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ContractsComponent } from '../contracts/contracts.component';
import { TeamsComponent } from '../teams/teams.component';
import { MatTableModule } from '@angular/material/table';
import { AppComponent } from './app/app.component';

@NgModule({
    declarations: [
        AppComponent,
        ContractsComponent,
        TeamsComponent
    ],
    imports: [
        CommonModule, BrowserModule, FormsModule,
        MatTableModule, MatPaginatorModule, MatSortModule,
        MatButtonModule, MatButtonToggleModule, MatToolbarModule,
        HttpClientModule,
        BrowserAnimationsModule
    ],
    providers: [],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
