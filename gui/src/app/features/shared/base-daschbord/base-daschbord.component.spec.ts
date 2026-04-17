import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseDaschbordComponent } from './base-daschbord.component';

describe('BaseDaschbordComponent', () => {
  let component: BaseDaschbordComponent;
  let fixture: ComponentFixture<BaseDaschbordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaseDaschbordComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaseDaschbordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
