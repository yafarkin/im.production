import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeamRoutingModule } from './team-routing.module';
import { ContractsComponent } from './contracts/contracts.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule, MatCardModule, MatDividerModule, MatGridListModule } from '@angular/material';
import { ContractComponent } from './contract/contract.component';
import { ContractsService } from './services/contracts.service';

@NgModule({
    declarations: [
        ContractsComponent,
        ContractComponent
    ],
    imports: [
        CommonModule,
        MatToolbarModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatTableModule,
        MatPaginatorModule,
        MatCardModule,
        MatDividerModule,
        MatGridListModule,
        TeamRoutingModule
    ],
    providers: [
        ContractsService
    ]
})
export class TeamModule { }
