import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  productForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private productService: ProductService) {
    this.productForm = this.fb.group({
      products: this.fb.array([this.createProductFormGroup()])
    });
  }


  createProductFormGroup(): FormGroup {
    return this.fb.group({
      image: ['', Validators.required],
      imageData: [''],
      title: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(250)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [0, [Validators.required, Validators.min(1)]],
      date: ['', Validators.required]
    });
  }

  products(): FormArray {
    return this.productForm.get('products') as FormArray;
  }


  addProduct(): void {
    this.products().push(this.createProductFormGroup());
  }

  removeProduct(index: number): void {
    this.products().removeAt(index);
  }

  onFileChange(event: any, index: number): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.products().at(index).get('imageData')?.setValue(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  }

  // Submit the form
  onSubmit(): void {
    this.submitted = true;
    if (this.productForm.invalid) {
      return;
    }
    const products = this.productForm.value.products.map((product: any) => ({
      ...product,
      date: this.convertToDateTime(product.date)
    }));

    const productData = this.productForm.value.products;
    this.productService.saveProducts(productData).subscribe(
      response => {
        alert('Products saved successfully');
        this.productForm.reset();
        this.submitted = false;
        const formArray = this.products();
        formArray.clear();
        formArray.push(this.createProductFormGroup());
        console.log('Products saved successfully:', response);
      },
      error => {
        alert('Error saving products');
        console.error('Error saving products:', error);
      });
  }

  convertToDateTime(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString();
  }

  hasError(controlName: string, index: number, errorName: string): boolean {
    const control = this.products().at(index).get(controlName);
    return (control?.hasError(errorName) || false) && (control?.dirty || this.submitted);
  }

}
