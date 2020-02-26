import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AddingFactoryService } from '../services/adding-factory.service';
import { AddingFactoryDto } from '../models/dtos/adding-factory.dto';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'app-adding-factory',
    templateUrl: './adding-factory.component.html',
    styleUrls: ['./adding-factory.component.scss'],
    providers: [AddingFactoryService]
})
export class AddingFactoryComponent implements OnInit {
    levels: number[] = [1, 2, 3];
    productionTypes: string[] = [
        "Металлургическая промышленность",
        "Электронная промышленность",
        "Дерево-обрататывающая промышленность",
        "Нефте-газо-химическая промышленность"
    ];
    maxWorkers: number = 0;
    addFactoryGroup = new FormGroup({
        level: new FormControl('', Validators.required),
        workersAmounts: new FormControl('', [
            Validators.required, Validators.pattern("[0-9]*"), Validators.minLength(1), Validators.maxLength(30)
        ]),
        productionType: new FormControl('', Validators.required),
        sum: new FormControl('', [
            Validators.required, Validators.pattern("[0-9]*"), Validators.minLength(1), Validators.maxLength(30)
        ]),
    });

    constructor(private addingFactoryService: AddingFactoryService, private snackBar: MatSnackBar) { }

    ngOnInit() {
    }

    addFactory() {
        let factory: AddingFactoryDto = new AddingFactoryDto();
        //TODO: Заполнить модель данными из FormGroup
        this.addingFactoryService.addFactory(factory).subscribe(
            success => {
                this.snackBar.open("Фабрика добавлена!", "", {
                    duration: 3 * 1000
                });
            },
            error => {
                this.snackBar.open("Фабрика не добавлена!", "", {
                    duration: 3 * 1000
                });
            }
        );
    }

    setMaxWorkers() {
        if (this.addFactoryGroup.value.level == 1) {
            this.maxWorkers = 10;
        } else if (this.addFactoryGroup.value.level == 2) {
            this.maxWorkers = 20;
        } else if (this.addFactoryGroup.value.level == 3) {
            this.maxWorkers = 30;
        }
    }
}
