import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationService } from './services/navigation.service';

@NgModule({
    imports: [
        CommonModule
    ],
    providers: [
        NavigationService
    ]
})
export class SharedModule { }
