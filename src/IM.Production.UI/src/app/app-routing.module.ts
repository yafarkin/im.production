import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccessGuard } from './features/access/guards/access.guard';
import { LoginComponent } from './features/access/login/login.component';
import { Roles } from './features/access/models/roles';

const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.module').then(v => v.AdminModule),
        canActivate: [AccessGuard],
        data: { role: Roles.admin }
    },
    {
        path: 'team',
        loadChildren: () => import('./features/team/team.module').then(v => v.TeamModule),
        canActivate: [AccessGuard],
        data: { role: Roles.team }
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
