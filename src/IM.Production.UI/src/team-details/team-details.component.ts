import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Team } from '../models/team';

@Component({
    selector: 'app-team-details',
    templateUrl: './team-details.component.html',
    styleUrls: ['./team-details.component.scss']
})
export class TeamDetailsComponent {
    name: string;
    productionType: string;
    factories: string[];
    sum: number;
    contracts: string;

    querySubscription: Subscription;

    constructor(private route: ActivatedRoute) {

        this.querySubscription = route.queryParams.subscribe(
            (queryParam: any) => {
                this.name = queryParam['name'];
                this.productionType = queryParam['productionType'];
                this.factories = queryParam['factories'];
                this.sum = queryParam['sum'];
                this.contracts = queryParam['contracts'];
            }
        );
    }
}
