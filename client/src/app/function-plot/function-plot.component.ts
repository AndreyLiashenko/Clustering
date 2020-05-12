import { Component, OnInit, Input } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import {KmeansService} from '../kmeans.services';
import {GaussResponse} from '../shared/gauss/gauss-response';
import {PlotFunctionModel} from '../shared/plot-function-model';

import * as functionPlot from '../../../node_modules/function-plot/dist/function-plot.js';
import { Centroid } from '../shared/kmeans/centroid.js';

@Component({
  selector: 'app-function-plot',
  templateUrl: './function-plot.component.html',
  styleUrls: ['./function-plot.component.css']
})
export class FunctionPlotComponent implements OnInit {
  
  @Input() centroids: Centroid;
  gaussResponse: GaussResponse[] =[];
  data: PlotFunctionModel[] = [];
  axis: number;

  constructor(private kmeansService: KmeansService) { 
    this.centroids = new Centroid();
  }

  ngOnInit(): void {
  }

   buildGraph(){
     this.kmeansService.getGaussianParam(this.centroids, this.axis)
     .subscribe(event => {
       if(event.type === HttpEventType.Response){
        this.gaussResponse = event.body;
        console.log('response', this.gaussResponse);
        this.data = this.mapData(this.gaussResponse);
        console.log('data', this.data);
        this.execPlotFunction(this.data);
       }
     });
    
    
   }

   execPlotFunction(data: PlotFunctionModel[]){
    const root = document.querySelector("#root");
    console.log('data', this.data);
    functionPlot({
      target: root,
      yAxis: { domain: [-0.5, 1.2] },
      xAxis: { domain: [1200, 3300] },
      tip: {
        renderer: function() {}
      },
      grid: true,
      data: data
    })
   }

    mapData(response: GaussResponse[]) : PlotFunctionModel[]{
    //  if(this.data.length != 0){
    //    this.data = [];
    //  }
        var arr : PlotFunctionModel[] = [];
        for(var item of response){
            var model = new PlotFunctionModel();
            model.fn = `exp(-((${item.mathWaiting}-x)^2)/${item.sigma})`;
            model.derivative = {
              fn: "2 * x",
              updateOnMouseMove: true
            }
            arr.push(model);
        }
        console.log('arr', arr);
        return arr;
   }
}
