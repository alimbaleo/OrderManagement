/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AppuserService } from './appuser.service';

describe('Service: Appuser', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppuserService]
    });
  });

  it('should ...', inject([AppuserService], (service: AppuserService) => {
    expect(service).toBeTruthy();
  }));
});
