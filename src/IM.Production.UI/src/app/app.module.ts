import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { TeamsGrid } from './teams-grid/teams-grid';

@NgModule({
  declarations: [
    AppComponent, TeamsGrid
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
