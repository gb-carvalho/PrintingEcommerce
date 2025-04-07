import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product/product.service';
import { AuthService } from '../../services/auth/auth.service'

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {

  products: any[] = [];

  constructor(private productService: ProductService, public authService: AuthService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: (data) => this.products = data,
      error: (err) => console.error('Erro ao carregar produtos', err)
    });
  }

  removeProduct(id: number) {
    if (confirm('Tem certeza que deseja remover este produto?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          alert('Produto removido com sucesso');
          this.products = this.products.filter(product => product.id !== id); // Remove da lista
        },
        error: (err) => {
          alert(`Erro ao remover produto: ${err.message || JSON.stringify(err)}`);
        },
        complete: () => {
          console.log('Requisição finalizada');
        }
      });
    }
  }
}
