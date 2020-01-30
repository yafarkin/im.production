import { Component, OnInit, ViewChild } from '@angular/core';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

import { Contract } from '../models/Contract';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {

  constructor() { }

  displayedColumns: string[] = [ 'position', 'fine', 'srcInsurancePremium', 'srcInsuranceAmount', 'destInsurancePremium', 'destInsuranceAmount', 'tillDate', 'tillCount', 'totalCountCompleted', 'totalSumm', 'totalOnTaxes'];
  arrayData: Contract[] = [
    { position: 1, fine: 10_000, srcInsurancePremium: 10_000, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 2, fine: 10_000, srcInsurancePremium: 11_000, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 3, fine: 10_000, srcInsurancePremium: 10_200, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 4, fine: 10_000, srcInsurancePremium: 10_400, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 5, fine: 10_000, srcInsurancePremium: 20_000, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 6, fine: 10_000, srcInsurancePremium: 43_000, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
    { position: 7, fine: 10_000, srcInsurancePremium: 12_000, srcInsuranceAmount: 15_000, destInsurancePremium: 100_000, destInsuranceAmount: 150_000, tillDate: 10122020, tillCount: 2500, totalCountCompleted: 25_000, totalSumm: 500_000, totalOnTaxes: 54_000 },
  ];

  dataSource = new MatTableDataSource<Contract>(this.arrayData);

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  
  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

}
