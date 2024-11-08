import { Component,OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-list-products',
  templateUrl: './list-products.component.html',
  styleUrl: './list-products.component.css'
})
export class ListProductsComponent implements OnInit {

  products: any[] = [];
  errorMessage: string = '';
  filteredProducts: any[] = [];
  p: number = 1;

  constructor(private productService: ProductService) {

  }

  ngOnInit(): void {
    this.productService.getProducts().subscribe(
      response => {
        this.products = response;
        this.filteredProducts = response;
      },
      error => {
        this.errorMessage = 'There was an error fetching the products.';
      });
  }

  filterTable(title: string, startDate: string, endDate: string): void {
    this.filteredProducts = this.products.filter(product => {
      const matchesTitle = title ? product.title.toLowerCase().includes(title.toLowerCase()) : true;
      let matchesDate = true;

      if (startDate && endDate) {
        const productDate = new Date(product.date);
        const filterStartDate = new Date(startDate);
        const filterEndDate = new Date(endDate);
        if (filterStartDate.getTime() === filterEndDate.getTime()) {
          matchesDate = productDate.toDateString() === filterStartDate.toDateString();
        } else {
          matchesDate = productDate >= filterStartDate && productDate <= filterEndDate;
        }
      }

      return matchesTitle && matchesDate;
    });
  };
}


