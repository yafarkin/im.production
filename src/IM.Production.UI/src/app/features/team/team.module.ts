import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeamRoutingModule } from './team-routing.module';
import { TeamHomeComponent } from './team-home/team-home.component';

@NgModule({
    declarations: [TeamHomeComponent],
    imports: [
        CommonModule,
        TeamRoutingModule
    ]
})
export class TeamModule { }
