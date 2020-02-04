import { Component, OnInit, ViewChild } from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort, MatSortable } from '@angular/material/sort';

import { Contract } from '../models/Customer/Contract';
import { ContractDto } from '../models/Dtos/ContractDto';
import { DateTime } from '../models/Net/DateTime';
import { ContractsService } from '../services/ContractsService.service';
import { Factory } from '../models/Production/Factory';
import { Customer } from '../models/Customer/Customer';
import { FactoryDto } from '../models/Dtos/FactoryDto';
import { CustomerDto } from '../models/Dtos/CustomerDto';
import { group } from '@angular/animations';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css'],
  providers: [ContractsService]
})
export class ContractsComponent implements OnInit {

  constructor(private contractsService: ContractsService) { }
  //,'sourceFactoryCustomerLogin', 'destinationFactoryCustomerLogin'
  displayedColumns: string[] = [ 
    'position', 'tillDate', 'tillCount', 'totalSumm'
     , 'sourceFactoryCustomerLogin' , 'destinationFactoryCustomerLogin'
  ];
  arrayData: ContractDto[] = [
    new ContractDto(0, 1, 2, 3, "login", "game")
  ];
  dataSource: MatTableDataSource<ContractDto> = new MatTableDataSource<ContractDto>(this.arrayData);
  toggleButtonFlag: string;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  sortPlayerAndGame(): void {
    let matSortable: MatSortable;
  }

  toggleButtonFlagChanged(): void {
    if (this.toggleButtonFlag === null || this.toggleButtonFlag.length <= 0) {
      return;
    }
    console.log("toggleButtonFlag: " + this.toggleButtonFlag);

    //"GamePlayerSale,GamePlayerBuy,PlayerPlayerSale"
    let gamePlayerSaleIndex: number = this.toggleButtonFlag.indexOf("GamePlayerSale");
    let gamePlayerBuyIndex: number = this.toggleButtonFlag.indexOf("GamePlayerBuy");
    let playerPlayerIndex: number = this.toggleButtonFlag.indexOf("PlayerPlayer");
    console.log("gamePlayerSaleIndex: " + gamePlayerSaleIndex);
    console.log("gamePlayerBuyIndex: " + gamePlayerBuyIndex);
    console.log("playerPlayerIndex: " + playerPlayerIndex);

    if (this.arrayData === null || this.arrayData.length <= 0) {
      return;
    }

    let filteredArrayData: ContractDto[] = [];
    for (let i: number = 0; i < this.arrayData.length; i++) 
    {
      let data: ContractDto = this.arrayData[i];
      if ((gamePlayerSaleIndex != -1 || gamePlayerBuyIndex != -1)
        && data.sourceFactoryCustomerLogin === "game" 
        && data.destinationFactoryCustomerLogin !== "game") 
      {
        filteredArrayData.push(data);
      }
      
      if (playerPlayerIndex != -1
        && data.sourceFactoryCustomerLogin !== "game" 
        && data.destinationFactoryCustomerLogin !== "game") 
      {
        filteredArrayData.push(data);
      }

    }
    
    this.dataSource = new MatTableDataSource<ContractDto>(filteredArrayData);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

    //if (splitted === null) {
    //  return;
    //}
    //console.log("splitted.length: " + splitted.length);
    //for (let i: number = 0; i < splitted.length; i++) {
    //  console.log("splitted: " + splitted[i]);
    //}
    
  }

  ngOnInit() {
    
    this.contractsService.getAllContracts().subscribe(
      (contracts) => {
        console.log("[success] contractsService.getAllContracts()");
        
        for (let i = 0; i < contracts.length; i++) 
        {
            let contract: ContractDto = contracts[i];
            if (contract.sourceFactoryCustomerLogin === null || 
                contract.destinationFactoryCustomerLogin === null)
            {
                continue;
            }
            this.arrayData.push(new ContractDto(i+1, contract.tillDate, contract.tillCount,
              contract.totalSumm, contract.sourceFactoryCustomerLogin, contract.destinationFactoryCustomerLogin));
        }

        this.dataSource = new MatTableDataSource<ContractDto>(this.arrayData);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;

        //let date: Date = new Date(contract.time.when);
        //contract.time.when = date;
        //this.debugContract(contract);
        
        // this.debugFactory(contract);
        
        //this.debugCustomer(contract);

      },
      (error) => {
        console.log("[error] contractsService.getAllContracts()" + error.message);
      }
    );
  }

}
