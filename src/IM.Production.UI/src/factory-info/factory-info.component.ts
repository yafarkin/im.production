import { Component, OnInit } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { FactoryContractDto } from '../models/dtos/factory.contract.dto';

@Component({
    selector: 'app-factory-info',
    templateUrl: './factory-info.component.html',
    styleUrls: ['./factory-info.component.scss'],
    providers: [ContractsService]
})
export class FactoryInfoComponent implements OnInit {

    displayedColumns: string[] = ['sourceCustomerLogin', 'destinationCustomerLogin', 'materialKey', 'totalCountCompleted', 'totalSumm'];
    oneTimeContractsSource: FactoryContractDto[] = [
        {
            sourceCustomerLogin: 'sourceCustomerLogin',
            destinationCustomerLogin: 'destinationCustomerLogin',
            materialKey: '',
            totalCountCompleted: 0,
            totalSumm: 200
        }
    ];

    multiTimeContractsSource: FactoryContractDto[] = [
        {
            sourceCustomerLogin: 'sourceCustomerLogin',
            destinationCustomerLogin: 'destinationCustomerLogin',
            materialKey: '',
            totalCountCompleted: 0,
            totalSumm: 200
        }
    ];

    constructor(private contractService: ContractsService) { }

    ngOnInit() {
        this.contractService.getOneTimeContracts("CustomerLogin1").subscribe(
            success => {
                this.oneTimeContractsSource = success
            }
        );
        this.contractService.getMultiTimeContracts("CustomerLogin1").subscribe(
            success => {
                this.multiTimeContractsSource = success;
            }
        );
    }

}
