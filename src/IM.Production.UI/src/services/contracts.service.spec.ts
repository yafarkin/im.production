/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ContractsService } from './ContractsService.service';

describe('Service: ContractsService', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [ContractsService]
        });
    });

    it('should ...', inject([ContractsService], (service: ContractsService) => {
        expect(service).toBeTruthy();
    }));
});
