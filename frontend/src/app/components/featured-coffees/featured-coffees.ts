import { Component, OnInit, signal } from '@angular/core';
import { CoffeeService } from '../../services/coffee';
import { Coffee } from '../../models/coffee.model';

@Component({
  selector: 'app-featured-coffees',
  imports: [],
  templateUrl: './featured-coffees.html',
  styleUrl: './featured-coffees.css'
})
export class FeaturedCoffees implements OnInit {
  coffees = signal<Coffee[]>([]);

  constructor(private coffeeService: CoffeeService) { }

  ngOnInit(): void {
    this.coffeeService.getCoffees().subscribe({
      next: (data) => this.coffees.set(data),
      error: (err) => console.error('Kahveler yüklenemedi:', err)
    });
  }
}