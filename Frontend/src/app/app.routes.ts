import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { ProductList } from './features/products/product-list/product-list';
import { InvoiceList } from './features/invoices/invoice-list/invoice-list';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { Register } from './features/auth/register/register';
import { authGuard } from './core/Guards/auth-guard';
import { AddProduct } from './features/products/add-product/add-product';
import { EditProduct } from './features/products/edit-product/edit-product';
import { roleGuard } from './core/Guards/role-guard';
import { CreateInvoice } from './features/invoices/create-invoice/create-invoice';
export const routes: Routes = [
    {path:'',redirectTo:'login',pathMatch:'full'},
    {path:'register',component:Register},
    {path:'login',component:Login},
    {path:'products',component:ProductList,canActivate:[authGuard]},
    {path:'dashboard',component:Dashboard,canActivate:[authGuard]},
    {path:'add-product',component:AddProduct,canActivate:[authGuard,roleGuard],data:{roles:['Admin','StoreManager']}},
    {path:'edit-product/:id',component:EditProduct,canActivate:[authGuard,roleGuard],data:{roles:['Admin','StoreManager']}},
    {path:'invoices',component:InvoiceList,canActivate:[authGuard]},
    {path:'create-invoice',component:CreateInvoice,canActivate:[authGuard]}
];
