import { Component, OnInit, ViewChild } from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { ContractDto } from '../models/dtos/contract.dto';
import { ContractsService } from '../services/contracts.service';

@Component({
    selector: 'app-contracts',
    templateUrl: './contracts.component.html',
    styleUrls: ['./contracts.component.scss'],
    providers: [ContractsService]
})
export class ContractsComponent implements OnInit {

    constructor(private contractsService: ContractsService) { }
    displayedColumns: string[] = [
        'index', 'tillDate', 'tillCount', 'totalSumm'
        , 'sourceFactoryCustomerLogin', 'destinationFactoryCustomerLogin'
    ];
    arrayData: ContractDto[] = [
    ];
    dataSource: MatTableDataSource<ContractDto> = new MatTableDataSource<ContractDto>(this.arrayData);
    toggleButtonFlag: string;

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    updateContracts(): void {
        this.contractsService.getContracts().subscribe(
            (contracts) => {
                this.arrayData = contracts;
                this.dataSource = new MatTableDataSource<ContractDto>(this.arrayData);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            }
        );
    }

    toggleButtonFlagChanged(): void {
        if (this.toggleButtonFlag === null || this.toggleButtonFlag.length <= 0) {
            this.dataSource = new MatTableDataSource<ContractDto>(this.arrayData);
            this.dataSource.sort = this.sort;
            this.dataSource.paginator = this.paginator;
            return;
        }

        let gamePlayerSaleIndex: number = this.toggleButtonFlag.indexOf("GamePlayerSale");
        let gamePlayerBuyIndex: number = this.toggleButtonFlag.indexOf("GamePlayerBuy");
        let playerPlayerIndex: number = this.toggleButtonFlag.indexOf("PlayerPlayer");

        if (this.arrayData === null || this.arrayData.length <= 0) {
            return;
        }

        let filteredArrayData: ContractDto[] = [];
        for (let i: number = 0; i < this.arrayData.length; i++) {
            let data: ContractDto = this.arrayData[i];
            if (gamePlayerSaleIndex != -1
                && data.sourceFactoryCustomerLogin === "Game"
                && data.destinationFactoryCustomerLogin !== "Game") {
                filteredArrayData.push(data);
            }

            if (gamePlayerBuyIndex != -1
                && data.sourceFactoryCustomerLogin !== "Game"
                && data.destinationFactoryCustomerLogin === "Game") {
                filteredArrayData.push(data);
            }

            if (playerPlayerIndex != -1
                && data.sourceFactoryCustomerLogin !== "Game"
                && data.destinationFactoryCustomerLogin !== "Game") {
                filteredArrayData.push(data);
            }
        }

        this.dataSource = new MatTableDataSource<ContractDto>(filteredArrayData);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    ngOnInit() {
        this.updateContracts();
    }

}
