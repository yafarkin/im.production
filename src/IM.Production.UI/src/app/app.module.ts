import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { TeamsGrid } from './teams-grid/teams-grid';
import {MatTableModule} from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent, TeamsGrid
  ],
  imports: [
    BrowserModule, MatTableModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
