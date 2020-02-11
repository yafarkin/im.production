import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { ContractDto } from '../models/dtos/contract.dto';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-contract',
    templateUrl: './contract.component.html',
    styleUrls: ['./contract.component.scss'],
    providers: [ContractsService]
})
export class ContractComponent implements OnInit {
    contractId: string;
    contract: ContractDto;

    constructor(private router: Router, private activateRoute: ActivatedRoute, private contractsService: ContractsService) {
        this.contractId = activateRoute.snapshot.params['id'];
    }

    ngOnInit() {
        this.contractsService.getContract(this.contractId).subscribe(
            contract => {
                this.contract = contract;
            }
        );
    }

    backToContractsClick(): void {
        this.router.navigateByUrl("/contracts");
    }
}
