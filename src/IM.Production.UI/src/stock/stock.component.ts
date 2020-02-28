import { Component, OnInit } from '@angular/core';
import { StockMaterialDto } from '../models/dtos/stock.material.dto';
import { MatTableDataSource } from '@angular/material/table';
import { StockService } from '../services/stock.service';

@Component({
    selector: 'app-stock',
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.scss'],
    providers: [StockService]
})
export class StockComponent implements OnInit {

    materials: StockMaterialDto[] = [];
    materialsSource: MatTableDataSource<StockMaterialDto>;

    constructor(private warehouseService: StockService) { }

    ngOnInit() {
        //TODO: remove it in the future
        let temp: StockMaterialDto = new StockMaterialDto();
        temp.name = "Material";
        temp.amount = 100;
        temp.progression = 100;
        temp.sellPrice = 100;
        this.materials.push(temp);

        this.warehouseService.getMaterials('{123}').subscribe(
            success => {
                this.materials = success;
            }
        );
        this.materialsSource = new MatTableDataSource<StockMaterialDto>(this.materials);
    }
}
