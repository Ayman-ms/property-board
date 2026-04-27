import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDashboarComponent } from './admin-dashboar.component';

describe('AdminDashboarComponent', () => {
  let component: AdminDashboarComponent;
  let fixture: ComponentFixture<AdminDashboarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminDashboarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDashboarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
