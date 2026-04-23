import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  apiUrl="https://localhost:7108/api/auth";
  constructor(private http:HttpClient){}
  register(user:any){
    return this.http.post(`${this.apiUrl}/register`,user);
  };
  login(data:any){
    return this.http.post(`${this.apiUrl}/login`,data);
  }
}
