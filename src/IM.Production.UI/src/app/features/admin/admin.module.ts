import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { TeamsComponent } from './teams/teams.component';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { GameManagementComponent } from './game-management/game-management.component';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { GameManagementService } from './services/game.management.service';
import { TeamsService } from './services/teams.service';
import { NewTeamComponent } from './new-team/new-team.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
    declarations: [
        TeamsComponent,
        NewTeamComponent,
        GameManagementComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatTableModule,
        MatButtonModule,
        MatCardModule,
        MatProgressBarModule,
        MatInputModule,
        MatGridListModule,
        MatDialogModule,
        MatSnackBarModule,
        AdminRoutingModule
    ],
    providers: [
        TeamsService,
        GameManagementService
    ],
    entryComponents: [
        NewTeamComponent
    ]
})
export class AdminModule { }
