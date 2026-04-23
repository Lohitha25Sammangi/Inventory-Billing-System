import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { Auth } from '../../../core/services/auth';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.html'
})
export class Register implements OnInit {
  registerForm!: FormGroup;

  constructor(private fb:FormBuilder, private authService:Auth){}
  ngOnInit(){
    this.registerForm = this.fb.group({
    FullName:['',Validators.required],
    Email:['',[Validators.required,Validators.email]],
    Password:['',Validators.required],
    Role:['Cashier']
  });

  }
  register(){

    if(this.registerForm.invalid){
      return;
    }

    this.authService.register(this.registerForm.value)
      .subscribe({
        next:(res)=>{
          console.log("User registered",res);
          alert("Registration successful");
        },
        error:(err)=>{
          console.log(err);
        }
      });

  }

}