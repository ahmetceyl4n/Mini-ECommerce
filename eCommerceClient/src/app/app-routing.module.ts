import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './admin/layout/layout.component';
import { HomeComponent } from './ui/components/home/home.component';
import { DashboardComponent } from './admin/components/dashboard/dashboard.component';

const routes: Routes = [
  {path: "admin", component: LayoutComponent, children: [
    {path: "", component: DashboardComponent},  //Burada path'in boş olmasının nedeni direkt admin sayfasına (https://bilmemne/admin) gidilmek istendiğinde dashboard'ın açılması istendiği için. 
    {path: "customers", loadChildren: () => import("./admin/components/customer/customer.module").then(module => module.CustomerModule)}, //https://bilmemne/admin/customers
    {path: "orders", loadChildren: () => import("./admin/components/order/order.module").then(module => module.OrderModule)},
    {path: "products", loadChildren: () => import("./admin/components/products/products.module").then(module => module.ProductsModule)}
  ]},
  {path: "", component: HomeComponent}, // Direkt https://bilmemne sitesine gidildiğinde açılacak sayfa
  {path: "basket", loadChildren: () => import("./ui/components/baskets/baskets.module").then(module => module.BasketsModule)},
  {path: "register", loadChildren: () => import("./ui/components/register/register.module").then(module => module.RegisterModule)},
  {path: "products", loadChildren: () => import("./ui/components/products/products.module").then(module => module.ProductsModule)},
  {path: "login", loadChildren: () => import("./ui/components/login/login.module").then(module => module.LoginModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
