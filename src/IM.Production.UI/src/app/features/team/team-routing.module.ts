import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContractsComponent } from './contracts/contracts.component';
import { TeamComponent } from './team/team.component';
import { StockComponent } from './stock/stock.component';

const routes: Routes = [
    {
        path: '',
        component: TeamComponent
    },
    {
        path: 'contracts',
        component: ContractsComponent
    },
    {
        path: 'stock',
        component: StockComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TeamRoutingModule { }
