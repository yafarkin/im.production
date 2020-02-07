import { Component, OnInit } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { ContractDto } from '../models/dtos/contract.dto';

@Component({
    selector: 'app-contract',
    templateUrl: './contract.component.html',
    styleUrls: ['./contract.component.scss'],
    providers: [ContractsService]
})
export class ContractComponent implements OnInit {
    contract: ContractDto;

    constructor(private contractsService: ContractsService) { }

    ngOnInit() {
        this.contractsService.getContract().subscribe(
            contract => {
                console.log("[success]");
                this.contract = contract;
            }
        )
    }
}
