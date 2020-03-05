import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app/app.component';
import { LoginComponent } from './login/login.component';
import { TeamComponent } from './features/team/team/team.component';
import { FactoriesComponent } from './features/team/factories/factories.component';
import { TeamService } from './features/team/services/team.service';

@NgModule({
    declarations: [
        AppComponent,
        TeamComponent,
        FactoriesComponent,
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
        MatButtonModule,
        MatInputModule
    ],
    providers: [
        TeamService
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { }
