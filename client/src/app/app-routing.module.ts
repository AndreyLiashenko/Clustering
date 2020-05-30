import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CleansingComponent } from './cleansing/cleansing.component';


const routes: Routes = [  { path: 'cleanse', component: CleansingComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
