import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeamHomeComponent } from './team-home/team-home.component';

const routes: Routes = [
    {
        path: '',
        component: TeamHomeComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TeamRoutingModule { }
