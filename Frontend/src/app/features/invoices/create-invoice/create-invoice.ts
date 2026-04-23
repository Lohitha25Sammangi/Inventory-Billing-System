import { Component, OnInit } from '@angular/core';
import { Product } from '../../../core/services/product';
import { Invoice } from '../../../core/services/invoice';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-invoice',
  imports: [FormsModule,CommonModule],
  templateUrl: './create-invoice.html',
  styleUrl: './create-invoice.css',
})
export class CreateInvoice implements OnInit {
  products:any[]=[];
  selectedProductId:number=0;
  quantity:number=1;
  invoiceItems:any[]=[];
  subtotal:number=0;
  tax:number=0;
  grandTotal:number=0;
  taxPercentage:number=10;
  constructor(private productService:Product, private invoiceService:Invoice
  ){}
  ngOnInit() {
    this.loadProducts();
  }
  loadProducts(){
    this.productService.getAllProducts()
    .subscribe({
      next:(res:any)=>{
      this.products=res;
      console.log("Products:",this.products);
    },
    error:(err)=>{
      console
      .log(err);
    }
  });
  }
  addItem(){
    const product=this.products.find(p=>p.id==this.selectedProductId);
    if(!product){
      alert("Select Product");
      return;
    }
    const total=product.price*this.quantity;
    this.taxPercentage=product.taxPercentage;
    this.invoiceItems.push({
      productId:product.id,
      name:product.name,
      price:product.price,
      quantity:this.quantity,
      total:total
    })
    this.calculateTotal();
  }
  calculateTotal(){
    this.subtotal=this.invoiceItems.reduce((sum,item)=>sum+item.total,0);
    this.tax=this.subtotal*this.taxPercentage/100;
    this.grandTotal=this.subtotal+this.tax;
  }
  generateInvoice(){
    const payload={
      items:this.invoiceItems.map(i=>({
        productId:i.productId,
        quantity:i.quantity
      }))
    }
    this.invoiceService.createInvoice(payload)
    .subscribe({
      next:()=>{
        alert("Invoice created successfully");
        this.invoiceItems=[];
        this.calculateTotal();

      },
      error:err=>{
        alert(err.error);
      }
    });
  }
}
