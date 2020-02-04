import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { ContractsComponent } from '../contracts/contracts.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
@NgModule({
   declarations: [
      AppComponent,
      ContractsComponent
   ],
   imports: [
      CommonModule, BrowserModule, FormsModule,
      MatTableModule, MatPaginatorModule, MatSortModule, MatButtonToggleModule,
      HttpClientModule,
      BrowserAnimationsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
