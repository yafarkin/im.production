import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

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

    constructor(private contractsService: ContractsService, private router: Router) { }
    gamePlayerSaleFlag: boolean = false;
    gamePlayerBuyFlag: boolean = false;
    playerPlayerFlag: boolean = false;
    displayedColumns: string[] = [
        'index', 'tillDate', 'tillCount', 'totalSumm', 'sourceCustomerLogin', 'sourceFactoryName', 'sourceGenerationLevel', 'sourceWorkers', 'destinationCustomerLogin', 'destinationFactoryName', 'destinationGenerationLevel', 'destinationWorkers'
    ];
    arrayData: ContractDto[] = [];
    filteredArrayData: ContractDto[] = [];
    thisArrayDataSource: MatTableDataSource<ContractDto>;
    dataSource: MatTableDataSource<ContractDto> = new MatTableDataSource<ContractDto>(this.arrayData);

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    routSwitchClick(contract: ContractDto): void {
        this.router.navigateByUrl('/contracts/' + contract.id);
    }

    getAllContracts(): void {
        this.contractsService.getAllContracts().subscribe(
            (contracts) => {
                this.arrayData = contracts;
                this.thisArrayDataSource = new MatTableDataSource<ContractDto>(this.arrayData);
                this.dataTableFilter();
            }
        );
    }

    gamePlayerSaleFlagChanged(): void {
        if (this.gamePlayerSaleFlag) {
            this.gamePlayerSaleFlag = false;
        }
        else {
            this.gamePlayerSaleFlag = true;
        }
        this.dataTableFilter();
    }

    gamePlayerBuyFlagChanged(): void {
        if (this.gamePlayerBuyFlag) {
            this.gamePlayerBuyFlag = false;
        }
        else {
            this.gamePlayerBuyFlag = true;
        }
        this.dataTableFilter();
    }

    playerPlayerFlagChanged(): void {
        if (this.playerPlayerFlag) {
            this.playerPlayerFlag = false;
        }
        else {
            this.playerPlayerFlag = true;
        }
        this.dataTableFilter();
    }

    dataTableFilter(): void {
        if (!this.gamePlayerSaleFlag
            && !this.gamePlayerBuyFlag
            && !this.playerPlayerFlag) {
            this.dataSource = this.thisArrayDataSource;
            this.dataSource.sort = this.sort;
            this.dataSource.paginator = this.paginator;
            return;
        }

        if (this.arrayData === null || this.arrayData.length <= 0) {
            return;
        }

        for (let data of this.arrayData) {
            if (this.gamePlayerSaleFlag
                && data.sourceCustomerLogin === "Game"
                && data.destinationCustomerLogin !== "Game") {
                this.filteredArrayData.push(data);
            } else if (this.gamePlayerBuyFlag
                && data.sourceCustomerLogin !== "Game"
                && data.destinationCustomerLogin === "Game") {
                this.filteredArrayData.push(data);
            } else if (this.playerPlayerFlag
                && data.sourceCustomerLogin !== "Game"
                && data.destinationCustomerLogin !== "Game") {
                this.filteredArrayData.push(data);
            }
        }

        this.dataSource = new MatTableDataSource<ContractDto>(this.filteredArrayData);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.filteredArrayData = [];
    }

    ngOnInit() {
        this.getAllContracts();
    }

}
