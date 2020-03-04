import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminHomeComponent } from './admin-home/admin-home.component';

@NgModule({
    declarations: [AdminHomeComponent],
    imports: [
        CommonModule,
        AdminRoutingModule
    ]
})
export class AdminModule { }
