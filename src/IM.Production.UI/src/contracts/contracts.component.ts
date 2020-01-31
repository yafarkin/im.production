import { Component, OnInit, ViewChild } from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { Contract } from '../models/Customer/Contract';
import { ContractsService } from '../services/ContractsService.service';

export class A {
  constructor(public f1:number)
  {
  }
}

export class B extends A {
  constructor(public name:any) 
  {
      super(21);
  }
}

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css'],
  providers: [ContractsService]
})
export class ContractsComponent implements OnInit {

  constructor(private contractsService: ContractsService) { }

  displayedColumns: string[] = [ 'position', 'fine', 'srcInsurancePremium', 'srcInsuranceAmount', 'destInsurancePremium', 'destInsuranceAmount', 'tillDate', 'tillCount', 'totalCountCompleted', 'totalSumm', 'totalOnTaxes'];
  arrayData: Contract[] = [];

  dataSource = new MatTableDataSource<Contract>(this.arrayData);

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  
  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

    for (let i = 0; i < 25; i++) {
      let constract: Contract = new Contract(i, i*111 , i*10_000, i*15_000, 
        i*100_000, i*150_000, 
        i*10122020, i*2500, i*25_000, i*50000, i*54_000, null, null, null, null);
      this.arrayData.push(constract);
    }

    this.contractsService.getAllContracts().subscribe(
      (contract) => {
        console.log("[success] contractsService.getAllContracts()");
        console.log("obj: " + contract);
      },
      (error) => {
        console.log("[error] contractsService.getAllContracts()" + error.message);
      }
    );
  }

}
