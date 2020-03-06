import { Component, OnInit } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { FactoryContractDto } from '../models/factory.contract.dto';
import { AuthenticationService } from '../../../services/authentication.service';

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

    constructor(private contractService: ContractsService, private auth: AuthenticationService) { }

    ngOnInit() {
        let login = this.auth.currentUser.login;
        this.contractService.getOneTimeContracts(login).subscribe(
            success => {
                this.oneTimeContractsSource = success
            }
        );
        this.contractService.getMultiTimeContracts(login).subscribe(
            success => {
                this.multiTimeContractsSource = success;
            }
        );
    }

}
