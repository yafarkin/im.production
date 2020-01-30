/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ContractsServiceService } from './ContractsService.service';

describe('Service: ContractsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ContractsServiceService]
    });
  });

  it('should ...', inject([ContractsServiceService], (service: ContractsServiceService) => {
    expect(service).toBeTruthy();
  }));
});
