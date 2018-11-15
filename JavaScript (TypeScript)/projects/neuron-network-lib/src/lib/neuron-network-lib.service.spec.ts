import { TestBed } from '@angular/core/testing';

import { NeuronNetworkLibService } from './neuron-network-lib.service';

describe('NeuronNetworkLibService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NeuronNetworkLibService = TestBed.get(NeuronNetworkLibService);
    expect(service).toBeTruthy();
  });
});
