import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMangeComponent } from './user-mange.component';

describe('UserMangeComponent', () => {
  let component: UserMangeComponent;
  let fixture: ComponentFixture<UserMangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserMangeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserMangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
