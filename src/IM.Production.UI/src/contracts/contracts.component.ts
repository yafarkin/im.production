import { Component, OnInit, ViewChild } from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { Contract } from '../models/Customer/Contract';
import { ContractDto } from '../models/Dtos/ContractDto';
import { DateTime } from '../models/Net/DateTime';
import { ContractsService } from '../services/ContractsService.service';
import { Factory } from '../models/Production/Factory';
import { Customer } from '../models/Customer/Customer';
import { FactoryDto } from '../models/Dtos/FactoryDto';
import { CustomerDto } from '../models/Dtos/CustomerDto';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css'],
  providers: [ContractsService]
})
export class ContractsComponent implements OnInit {

  constructor(private contractsService: ContractsService) { }
  //'sourceFactoryCustomerLogin', 'destinationFactoryCustomerLogin'
  displayedColumns: string[] = [ 'position', 'tillDate', 'tillCount', 'totalSumm'];
  arrayData: ContractDto[] = [
    new ContractDto(0, 12,12,343, "login", "game")
  ];
  dataSource: MatTableDataSource<ContractDto> = new MatTableDataSource<ContractDto>(this.arrayData);

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  
  debugContract(contract: Contract): void
  {
    console.log("Print contract");
    console.log("contract.customer: " + contract.customer);
    console.log("contract.description: " + contract.description);
    console.log("contract.destInsuranceAmount: " + contract.destInsuranceAmount);
    console.log("contract.destInsurancePremium: " + contract.destInsurancePremium);
    console.log("contract.fine: " + contract.fine);
    console.log("contract.materialWithPrice: " + contract.materialWithPrice);
    console.log("contract.position: " + contract.position);
    console.log("contract.sourceFactory: " + contract.sourceFactory);
    console.log("contract.srcInsuranceAmount: " + contract.srcInsuranceAmount);
    console.log("contract.srcInsurancePremium: " + contract.srcInsurancePremium);
    console.log("contract.tax: " + contract.tax);
    console.log("contract.tillCount: " + contract.tillCount);
    console.log("contract.tillDate: " + contract.tillDate);
    console.log("contract.time: " + contract.time);
    console.log("contract.time.day: " + contract.time.day);
    console.log("contract.time.when: " + contract.time.when);
    console.log("contract.time.when.month: " + contract.time.when.getUTCMonth());
    console.log("contract.time.when.year: " + contract.time.when.getUTCFullYear());
    console.log("contract.totalCountCompleted: " + contract.totalCountCompleted);
    console.log("contract.totalOnTaxes: " + contract.totalOnTaxes);
    console.log("contract.totalSumm: " + contract.totalSumm);
  }

  debugFactory(factory: Factory): void
  {
    console.log("Print factory");
    console.log("obj: " + factory);
    console.log("factory.SumOnRD: " + factory.SumOnRD);
    console.log("factory.customer: " + factory.customer);
    console.log("factory.displayName: " + factory.displayName);
    console.log("factory.factoryDefinition: " + factory.factoryDefinition);
    console.log("factory.id: " + factory.id);
    console.log("factory.level: " + factory.level);
    console.log("factory.needSumToNextLevelUp: " + factory.needSumToNextLevelUp);
    console.log("factory.performance: " + factory.performance);
    console.log("factory.productionMaterials: " + factory.productionMaterials);
    console.log("factory.rDProgress: " + factory.rDProgress);
    console.log("factory.readyForNextLevel: " + factory.readyForNextLevel);
    console.log("factory.spentSumToNextLevelUp: " + factory.spentSumToNextLevelUp);
    console.log("factory.stock: " + factory.stock);
    console.log("factory.totalExpenses: " + factory.totalExpenses);
    console.log("factory.totalOnRD: " + factory.totalOnRD);
    console.log("factory.totalOnSalary: " + factory.totalOnSalary);
    console.log("factory.totalOnTaxes: " + factory.totalOnTaxes);
    console.log("factory.workers: " + factory.workers);
  }

  debugCustomer(customer: Customer): void
  {
    console.log("Print customer");
    console.log("obj: " + customer);
    console.log("customer.bankFinanceOperations: " + customer.bankFinanceOperations);
    console.log("customer.contracts: " + customer.contracts);
    console.log("customer.displayName: " + customer.displayName);
    console.log("customer.factories: " + customer.factories);
    console.log("customer.factoryGenerationLevel: " + customer.factoryGenerationLevel);
    console.log("customer.id: " + customer.id);
    console.log("customer.login: " + customer.login);
    console.log("customer.passwordHash: " + customer.passwordHash);
    console.log("customer.productionType: " + customer.productionType);
    console.log("customer.rDProgress: " + customer.rDProgress);
    console.log("customer.readyForNextGenerationLevel: " + customer.readyForNextGenerationLevel);
    console.log("customer.spentSumToNextGenerationLevel: " + customer.spentSumToNextGenerationLevel);
    console.log("customer.sum: " + customer.sum);
    console.log("customer.sumOnRD: " + customer.sumOnRD);
    console.log("customer.sumToNextGeneration: " + customer.sumToNextGeneration);
  }

  ngOnInit() {
    
    this.contractsService.getAllContracts().subscribe(
      (contracts) => {
        console.log("[success] contractsService.getAllContracts()");
        
        for (let i = 0; i < contracts.length; i++) {
          contracts[i].position = i+1;
          if (contracts[i].sourceFactoryCustomerLogin == null || 
            contracts[i].destenationFactoryCustomerLogin == null)
          {
              continue;
          }
          this.arrayData.push(contracts[i]);
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
