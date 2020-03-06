import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app/app.component';
import { AccessModule } from './features/access/access.module';
import { SharedModule } from './features/shared/shared.module';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        AccessModule,
        SharedModule
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
