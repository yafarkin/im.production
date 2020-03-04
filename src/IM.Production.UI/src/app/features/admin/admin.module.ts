import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { TeamsComponent } from './teams/teams.component';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
    declarations: [TeamsComponent],
    imports: [
        CommonModule,
        MatTableModule,
        MatButtonModule,
        AdminRoutingModule
    ]
})
export class AdminModule { }
