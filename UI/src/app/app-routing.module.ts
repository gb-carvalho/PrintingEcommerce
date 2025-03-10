import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProductListComponent } from './components/product-list/product-list.component'
import { ContactComponent } from './components/contact/contact.component'
import { ProductFormComponent } from './components/product-form/product-form.component'
import { ProductListAdminComponent } from './components/product-list-admin/product-list-admin.component'

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'product-list', component: ProductListComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'product-form', component: ProductFormComponent },
  { path: 'product-list-admin', component: ProductListAdminComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
