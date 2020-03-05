import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule, MatButtonModule, MatCardModule, MatDividerModule, MatGridListModule } from '@angular/material';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';

import { TeamRoutingModule } from './team-routing.module';
import { TeamComponent } from './team/team.component';
import { ContractComponent } from './contract/contract.component';
import { ContractsComponent } from './contracts/contracts.component';
import { ContractsService } from './services/contracts.service';
import { FactoriesComponent } from './factories/factories.component';

@NgModule({
    declarations: [
        ContractsComponent,
        ContractComponent,
        TeamComponent,
        FactoriesComponent
    ],
    imports: [
        CommonModule,
        MatToolbarModule,
        MatTabsModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatTableModule,
        MatPaginatorModule,
        MatCardModule,
        MatDividerModule,
        MatGridListModule,
        TeamRoutingModule,
        MatListModule
    ],
    providers: [
        ContractsService
    ]
})
export class TeamModule { }
