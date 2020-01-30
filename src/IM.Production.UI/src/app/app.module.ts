import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { ContractsComponent } from '../contracts/contracts.component';

@NgModule({
   declarations: [
      AppComponent,
      ContractsComponent
   ],
   imports: [
      BrowserModule,
      MatTableModule, MatPaginatorModule, MatSortModule,
      HttpClientModule,
      BrowserAnimationsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
