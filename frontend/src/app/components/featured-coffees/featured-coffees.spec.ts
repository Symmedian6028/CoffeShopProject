import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturedCoffees } from './featured-coffees';

describe('FeaturedCoffees', () => {
  let component: FeaturedCoffees;
  let fixture: ComponentFixture<FeaturedCoffees>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeaturedCoffees],
    }).compileComponents();

    fixture = TestBed.createComponent(FeaturedCoffees);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
