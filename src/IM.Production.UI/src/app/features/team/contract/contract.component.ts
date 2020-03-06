import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ContractsService } from '../services/contracts.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ContractDto } from '../models/contract.dto';

@Component({
    selector: 'app-contract',
    templateUrl: './contract.component.html',
    styleUrls: ['./contract.component.scss']
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
