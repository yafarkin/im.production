import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { OldAppComponent } from './old-app/app.component';
import { AppComponent } from './app/app.component';
import { LoginComponent } from './login/login.component';

@NgModule({
    declarations: [
        OldAppComponent,
        AppComponent,
        LoginComponent
    ],
    imports: [
        BrowserModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
