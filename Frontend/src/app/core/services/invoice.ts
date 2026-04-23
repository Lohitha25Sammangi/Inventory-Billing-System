import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Invoice {
  private apiUrl='https://localhost:7108/api/Invoice';
  constructor(private http:HttpClient){}
  createInvoice(invoice:any):Observable<any>{
    return this.http.post(`${this.apiUrl}`,invoice);
  }
}
