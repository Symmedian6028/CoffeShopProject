import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './components/header/header';
import { Hero } from './components/hero/hero';
import { FeaturedCoffees } from './components/featured-coffees/featured-coffees';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Hero, FeaturedCoffees],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'coffee-shop-client';
}