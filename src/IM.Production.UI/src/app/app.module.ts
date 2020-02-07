import { NgModule } from '@angular/core';
<<<<<<< HEAD
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
import { AppComponent } from './app.component';
import { ContractsComponent } from '../contracts/contracts.component';
=======
import { AppComponent } from './app.component';
import { TeamsComponent } from '../teams/teams.component';
import { TeamsService } from '../services/teams.service';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
>>>>>>> d2f996465694edb0ccf1f595191be1d3e642e6e7

@NgModule({
    declarations: [
        AppComponent,
<<<<<<< HEAD
        ContractsComponent
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
=======
        TeamsComponent
    ],
    imports: [
        BrowserModule,
        MatTableModule,
        HttpClientModule
    ],
    providers: [TeamsService],
    bootstrap: [AppComponent]
>>>>>>> d2f996465694edb0ccf1f595191be1d3e642e6e7
})
export class AppModule { }
