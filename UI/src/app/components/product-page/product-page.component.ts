import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService, Product } from '../../services/product/product.service';


@Component({
  selector: 'app-product-page',
  standalone: false,
  templateUrl: './product-page.component.html',
  styleUrl: './product-page.component.css'
})
export class ProductPageComponent {
  productId: string | null = null;
  product: Product | null = null;
  quantity: number = 1;
  constructor(private productService: ProductService, private route: ActivatedRoute) { }

  get whatsappLink(): string {
    if (!this.product) return '';
    const total = this.product.price * this.quantity;
    const text = `Olá! Gostaria de finalizar o pedido de ${this.quantity} unidade(s) do produto ${this.product.name} por R$${total.toFixed(2)}.`;
    return `https://wa.me/5521987999131?text=${encodeURIComponent(text)}`;
  }

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');
    this.productService.getProduct(Number(this.productId)).subscribe({
      next: (product) => {
        this.product = product;
      },
      error: (err) => {
        console.error('Erro ao carregar produto:', err);  
      }
    });
  }
}
