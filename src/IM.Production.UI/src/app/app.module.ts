import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { TeamsComponent } from './teams.component';
import { TeamsService } from './services/teams.service';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent, TeamsComponent
  ],
  imports: [
    BrowserModule, MatTableModule
  ],
  providers: [TeamsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
