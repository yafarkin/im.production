import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule, MatInputModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthenticationService } from './services/authentication.service';
import { AccessGuard } from './guards/access.guard';

@NgModule({
    declarations: [LoginComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatButtonModule,
        MatInputModule,
        MatCardModule
    ],
    providers: [
        AuthenticationService,
        AccessGuard
    ]
})
export class AccessModule { }
