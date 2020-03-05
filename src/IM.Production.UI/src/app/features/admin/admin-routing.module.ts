import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeamsComponent } from './teams/teams.component';
import { GameManagementComponent } from './game-management/game-management.component';

const routes: Routes = [
    {
        path: '',
        component: TeamsComponent
    },
    {
        path: 'game',
        component: GameManagementComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule { }
