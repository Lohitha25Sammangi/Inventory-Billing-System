import { Inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const roleGuard: CanActivateFn = (route) => {
  const router=Inject(Router);
  if(typeof window !== undefined){
    const userRole=localStorage.getItem("role");
    const allowedRoles=route.data?.['roles'];
    if(allowedRoles.includes(userRole)){
      return true;
    }
    else{
      router.navigate['/products'];
      return false;
    }
  }
  return false;
};
