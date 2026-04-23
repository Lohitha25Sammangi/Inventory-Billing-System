import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../../core/services/product';

@Component({
  selector: 'app-edit-product',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css',
})
export class EditProduct implements OnInit {
  productId!:number;
  productForm!:FormGroup;
  constructor(
    private fb:FormBuilder,
    private route: ActivatedRoute,
    private router:Router,
    private productService:Product
  ){}
  ngOnInit(): void {
    this.productId=Number(this.route.snapshot.paramMap.get('id'));
  this.productService.getById(this.productId)
  .subscribe((data:any)=>{
    this.productForm.patchValue(data);
  });
    this.productForm=this.fb.group({
    name:['',Validators.required],
    price:['',Validators.required],
    stockQuantity:['',Validators.required],
    categoryId:['',Validators.required],
    taxPercentage:['',Validators.required],
    isActive:['',Validators.required],
  });
  }
  onSubmit(){
    console.log("Submit clicked");
    if(this.productForm.valid){
      this.productService.updateProduct(this.productId,this.productForm.value)
      .subscribe({
      next:(res)=>{
        console.log("Update Response",res);
        alert("Product Upadated Successfully");
        this.router.navigate(['/products']);
      },
      error:(err)=>{
        console.log("Update Error:",err);
      }
    });
    }
    
  }

}
