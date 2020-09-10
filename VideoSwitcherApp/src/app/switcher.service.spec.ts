import { TestBed } from '@angular/core/testing';

import { SwitcherService } from './switcher.service';

describe('SwitcherService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SwitcherService = TestBed.get(SwitcherService);
    expect(service).toBeTruthy();
  });
});
