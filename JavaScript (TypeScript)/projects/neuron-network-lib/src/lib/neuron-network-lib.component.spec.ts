import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NeuronNetworkLibComponent } from './neuron-network-lib.component';

describe('NeuronNetworkLibComponent', () => {
  let component: NeuronNetworkLibComponent;
  let fixture: ComponentFixture<NeuronNetworkLibComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NeuronNetworkLibComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NeuronNetworkLibComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
