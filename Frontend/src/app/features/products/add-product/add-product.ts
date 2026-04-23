import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Product } from '../../../core/services/product';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './add-product.html',
  styleUrl: './add-product.css',
})
export class AddProduct implements OnInit {
  productForm!:FormGroup;
  constructor(private fb:FormBuilder,private productService:Product,private router:Router){}
  ngOnInit() {
    this.productForm=this.fb.group({
    name:['',Validators.required],
    price:['',Validators.required],
    stockQuantity:['',Validators.required],
    categoryId:['',Validators.required],
    taxPercentage:['',Validators.required],
  });
  }
  onSubmit(){
    if(this.productForm.invalid){
      return;
    }
    this.productService.createProduct(this.productForm.value)
      .subscribe({
        next:(res)=>{
          alert("Product Added successfully");
          this.router.navigate(['/products']);
        },
        error:(err)=>{
          console.log(err);
        }
      });
  }
  
}
