import { Component } from '@angular/core';
import { ProductService } from '../../services/product/product.service';

@Component({
  selector: 'app-product-form',
  standalone: false,
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent {
  product = {
    name: '',
    price: 0,
    description: ''
  }
  constructor(private productService: ProductService) { }

  submitProduct() {
    this.productService.createProduct(this.product).subscribe(response => {
      console.log('Produto criado:', response);
      alert('Produto cadastrado com sucesso!');
    });
  }
}
