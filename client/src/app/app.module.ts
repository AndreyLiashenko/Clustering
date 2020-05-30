import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http'
import { AppComponent } from './app.component';
import { CleansingComponent } from './cleansing/cleansing.component';
import { Routes, RouterModule, Router } from '@angular/router';
import * as PlotlyJS from 'plotly.js/dist/plotly.js';
import { PlotlyModule } from 'angular-plotly.js';

const appRoutes: Routes = [
  { path: 'cleanse', component: CleansingComponent }
];

import { UploadComponent } from './upload/upload.component';
//import { FunctionPlotComponent } from './function-plot/function-plot.component';
import { ClusteringComponent } from './clustering/clustering.component';

PlotlyModule.plotlyjs = PlotlyJS;

@NgModule({
  declarations: [
    AppComponent,
    UploadComponent,
    //FunctionPlotComponent,
    CleansingComponent,
    ClusteringComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    CommonModule, 
    PlotlyModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
