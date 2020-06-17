import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { StockMaterialDto } from '../models/stock.material.dto';
import { StockService } from '../services/stock.service';
import { AuthenticationService } from '../../../services/authentication.service';
import { NavigationService } from '../../../services/navigation.service';

@Component({
    selector: 'app-stock',
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.scss'],
    providers: [StockService]
})
export class StockComponent implements OnInit {
    factoryId: string;
    materialsSource: MatTableDataSource<StockMaterialDto>;
    displayedColumns: string[] = [
        'key', 'productionType', 'amount', 'produceAmountPerDay', 'sellAmountPerDay'
    ];

    constructor(private stockService: StockService, private navigationService: NavigationService, private activateRoute: ActivatedRoute, private authService: AuthenticationService)
    {
        this.factoryId = activateRoute.snapshot.params['id'];
    }

    ngOnInit() {
        this.materialsSource = new MatTableDataSource<StockMaterialDto>();
        this.stockService.getMaterials(this.authService.currentUser.login, this.factoryId).subscribe(
            success => {
                this.materialsSource.data = success;
            }
        );
    }

    navigateToFactories(): void {
        this.navigationService.navigateToUrl("team/");
    }
}
