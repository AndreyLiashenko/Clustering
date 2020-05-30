import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CleansingComponent } from './cleansing/cleansing.component';
import { ClusteringComponent } from './clustering/clustering.component'


const routes: Routes = [  
  { path: 'cleanse', component: CleansingComponent },
  { path: 'cluster', component: ClusteringComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
