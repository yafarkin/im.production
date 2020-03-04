import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBarContainer } from '@angular/material';
import { Md5 } from 'ts-md5/dist/md5';

import { ContractsService } from '../services/contracts.service';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app/app.component';
import { ContractComponent } from '../contract/contract.component';
import { ContractsComponent } from '../contracts/contracts.component';

@NgModule({
    declarations: [
        AppComponent,
        ContractComponent,
        ContractsComponent,
        LoginComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        AppRoutingModule,
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
    ],
    providers: [
        ContractsService,
        Md5
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
