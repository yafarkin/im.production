import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app/app.component';
import { AccessGuard } from './core/access.guard';
import { LoginComponent } from './login/login.component';
import { Roles } from './models/roles';

const routes: Routes = [
    {
        path: '',
        component: AppComponent,
        canActivate: [AccessGuard],
        data: { roles: [Roles.admin, Roles.team] }
    },
    {
        path: 'login',
        component: LoginComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
