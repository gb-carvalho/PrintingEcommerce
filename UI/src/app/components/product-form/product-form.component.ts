import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product/product.service';

@Component({
  selector: 'app-product-form',
  standalone: false,
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent {
  product = {
    id: 0,
    name: '',
    price: 0,
    description: ''
  }
  isEdit: boolean = false;
  constructor(private productService: ProductService, private route: ActivatedRoute) { }

  ngOnInit() {
    const paramId = this.route.snapshot.paramMap.get('id')
    this.product.id = paramId ? +paramId : 0;
    

    if (this.product.id) {
      this.isEdit = true;
      this.productService.getProduct(this.product.id).subscribe({
        next: (user) => {
          //console.log(user.email);
          this.product.id = user.id;
          this.product.name = user.name;
          this.product.price = user.price;
          this.product.description = user.description;
        },
        error: (err) => {
          console.error('Erro ao buscar produto', err);
        }
      });
    }
    
  }

  submitProduct() {

    if (this.isEdit) {
      this.productService.updateProduct(this.product).subscribe(response => {
        console.log('Produto editado:', response);
        alert('Produto editado com sucesso!');
      });
    }
    else { 
      this.productService.createProduct(this.product).subscribe(response => {
        console.log('Produto criado:', response);
        alert('Produto cadastrado com sucesso!');
      });
    }
  }
}
