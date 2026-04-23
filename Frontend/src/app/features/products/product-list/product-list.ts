import { ChangeDetectorRef, Component,OnInit } from '@angular/core';
import { Product } from '../../../core/services/product';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  products:any[]=[];
  role:string | null='';
  constructor(private productService:Product,private cd:ChangeDetectorRef,private router:Router){}
  ngOnInit() {
    this.role = localStorage.getItem("role");
    this.loadProducts();
  }
  loadProducts(){
    this.productService.getAllProducts().subscribe(
      (data:any)=>{
        this.products=[...data];
        console.log(this.products);
        this.cd.detectChanges();
      });
  }
  editProduct(id:number){
    this.router.navigate(['/edit-product',id]);
  };
  deleteProduct(id:number){
    if(confirm("Are you sure you want to delete this product?")){
      this.productService.deleteProduct(id)
      .subscribe({
        next:(res)=>{
          alert("Product deleted successfully");
          this.loadProducts();
        },
        error:(err)=>{
          console.log(err);
        }
      });
    }
  
  };
    
}
