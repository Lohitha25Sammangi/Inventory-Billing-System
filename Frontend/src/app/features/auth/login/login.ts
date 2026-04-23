import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../../core/services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  loginForm!:FormGroup;
  constructor(private fb:FormBuilder, private authService:Auth,private router:Router){}
  ngOnInit(){
    this.loginForm=this.fb.group({
      Email:['',[Validators.required,Validators.email]],
      Password:['',Validators.required]
    });
  }
  login(){
    if(this.loginForm.invalid){
      return;
    }
    this.authService.login(this.loginForm.value)
    .subscribe({
      next:(res:any)=>{
        console.log("Login Success",res);
        localStorage.setItem("token",res.token);
        localStorage.setItem("role",res.roles);
        alert("Login Successfull");
        this.router.navigate(['/products']);
      },
      error:(err:any)=>{
          console.log(err);
        }
      
    });
  }
}
