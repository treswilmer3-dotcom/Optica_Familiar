import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BrandService } from './core/brand/brand.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: '<router-outlet />'
})
export class AppComponent {
  // Se instancia al arrancar para aplicar la última identidad visual conocida (también en el login).
  constructor() { inject(BrandService); }
}
