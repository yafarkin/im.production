import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-adding-factory',
    templateUrl: './adding-factory.component.html',
    styleUrls: ['./adding-factory.component.scss']
})
export class AddingFactoryComponent implements OnInit {
    levels: number[] = [1, 2, 3];
    numWorkers: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    factoryDefinitions: string[] = [
        "Металлургическая промышленность",
        "Электронная промышленность",
        "Химическая промышленность"
    ];

    constructor() { }

    ngOnInit() {
    }

}
