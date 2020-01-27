import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { OldAppComponent } from './old-app/app.component';
import { AppComponent } from './app/app.component';

@NgModule({
  declarations: [
    OldAppComponent,
    AppComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
