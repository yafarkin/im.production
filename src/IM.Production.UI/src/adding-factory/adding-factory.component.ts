import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ProductionType } from '../models/dtos/production.type.enum';
import { FactoryDefinition } from '../models/dtos/factory.definition';
import { TeamService } from '../services/team.service';

@Component({
    selector: 'app-adding-factory',
    templateUrl: './adding-factory.component.html',
    styleUrls: ['./adding-factory.component.scss'],
    providers: [TeamService]
})
export class AddingFactoryComponent implements OnInit {
    public login: string = "CustomerLogin0";
    factoriesDefinitions: FactoryDefinition[];
    currentIndex: number;
    currentFactory: FactoryDefinition;

    constructor(private teamService: TeamService, private snackBar: MatSnackBar) { }

    ngOnInit() {
        this.currentIndex = 0;
        this.getFactoriesDefinitions();
    }

    public getFactoriesDefinitions(): void {
        this.teamService.getFactoriesDefinitions(this.login).subscribe(
            success => {
                this.factoriesDefinitions = success;
                this.currentFactory = this.factoriesDefinitions[this.currentIndex];
            },
            error => {
                this.snackBar.open("Не удалось получить фабрики!", "", {
                    duration: 3 * 1000
                });
            }
        )
    }

    public buyFactory(definitionIndex: number): void {
        this.teamService.buyFactory(this.login, definitionIndex).subscribe(
            success => {
                this.snackBar.open("Фабрика куплена.", "", {
                    duration: 3 * 1000
                });
            },
            error => {
                this.snackBar.open("Не удалось купить фабрику!", "", {
                    duration: 3 * 1000
                });
            }
        )
    }

    public selectPrevious(): void {
        if (this.currentIndex > 0) {
            --this.currentIndex;
        }
    }

    public selectNext(): void {
        if (this.currentIndex <  (this.factoriesDefinitions.length - 1)) {
            ++this.currentIndex;
        }
    }
}
