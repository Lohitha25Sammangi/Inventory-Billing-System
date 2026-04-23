import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class Product {
  private apiUrl='https://localhost:7108/api/products';
  constructor(private http:HttpClient){}
  getAllProducts():Observable<any>{
    return this.http.get(`${this.apiUrl}`);
  };
  getById(id:number){
    return this.http.get(`${this.apiUrl}/${id}`);
  };
  createProduct(product:any):Observable<any>{
    return this.http.post(`${this.apiUrl}/Create`,product);
  };
  deleteProduct(id:number){
    return this.http.delete(`${this.apiUrl}/${id}`,{responseType:"text"});
  };
  updateProduct(id:number,product:any){
    return this.http.put(`${this.apiUrl}/Update/${id}`,product);
  }
}
