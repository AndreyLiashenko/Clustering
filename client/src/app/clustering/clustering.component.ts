import { Component, OnInit } from '@angular/core';
import {PlotlyClusteringService} from '../plotly-clustering.services';
import {KmeansModel} from '../shared/kmeans/kmeans-model'
import {Graph} from '../shared/plotly-clustering-model/graph'
import {Cluster} from '../shared/kmeans/cluster';
import {PlotFunctionModel} from '../shared/plot-function-model';
import {KmeansService} from '../kmeans.services';
import {GaussResponse} from '../shared/gauss/gauss-response';
import * as functionPlot from '../../../node_modules/function-plot/dist/function-plot.js';

import { Centroid } from '../shared/kmeans/centroid';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-clustering',
  templateUrl: './clustering.component.html'
})
export class ClusteringComponent implements OnInit {

  gaussResponse: GaussResponse[] =[];
  data: PlotFunctionModel[] = [];
  x1: number;
  x2: number;
  y1: number = -0.1;
  y2: number = 1;
  axis: number;
  graph: Graph;


    isOneCluster: boolean = false;
    numberOfCluster: number;
    listOfCluster: Cluster[] = [];
    centroids : Centroid;

  constructor(
    private plotlyClusteringService : PlotlyClusteringService,
    private kmeansService: KmeansService
  ) { 
    this.centroids = new Centroid();
  }

  ngOnInit(): void {
    this.graph = new Graph();
    this.graph =  this.plotlyClusteringService.oneClustering('rgb(23, 190, 207)',[{x: 1, y: 7, z: 3}]);
  }

  updateClusters(kmeansModel: KmeansModel){
    console.log('list of points', kmeansModel.listOfPoint)
    this.centroids = kmeansModel.centroids;

      console.log('number of clusters', this.numberOfCluster);
      for(var i = 0; i < this.numberOfCluster; i++){
          var clusterView = new Cluster();
          var red = Math.floor(Math.random() * 256);
          var green = Math.floor(Math.random() * 256);
          var blue = Math.floor(Math.random() * 256);
          var rgbColor = `rgb(${red}, ${green}, ${blue})`;
          var cluster = kmeansModel.listOfPoint.filter(x => x.clusterNumber === i);

          clusterView.clusterId = i;
          clusterView.rgbColor = rgbColor;
          clusterView.listOfPoint = cluster;
          this.listOfCluster.push(clusterView);
      }

      this.graph = this.plotlyClusteringService.allClustering(this.listOfCluster);
}

initialNumber(number: number){
  this.numberOfCluster = number;
  console.log('Number of cluster',this.numberOfCluster)
}

public onChange(event) {  
  const newVal = event.target.value;
  console.log('list of cluster', this.listOfCluster);
  console.log('newVal', newVal);
  var item = this.listOfCluster.find(x => x.clusterId == newVal);
  console.log('item', item)
  this.isOneCluster = true;
  console.log('IsOneCluster', this.isOneCluster);
  this.graph = this.plotlyClusteringService.oneClustering(item.rgbColor, item.listOfPoint);
  }
  
  public showAllClusters(){
  this.graph = this.plotlyClusteringService.allClustering(this.listOfCluster);
  this.isOneCluster = false;
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
   functionPlot({
     target: root,
     yAxis: { domain: [this.y1, this.y2] },
     xAxis: { domain: [1200, 3300] },
     tip: {
       renderer: function() {}
     },
     grid: true,
     data: data
   })
  }

   mapData(response: GaussResponse[]) : PlotFunctionModel[]{

       var arr : PlotFunctionModel[] = [];
       for(var item of response){
           var model = new PlotFunctionModel();
           console.log('sigma',item.sigma);
           model.fn = `exp(-((${item.mathWaiting}-x)^2)/${item.sigma})`;
           model.derivative = {
             fn: "2 * x",
             updateOnMouseMove: true
           }
           arr.push(model);
       }
       return arr;
  }

  changeScope(){
   const root = document.querySelector("#root");
   console.log('y1', this.y1);
   console.log('y2', this.y2);
   console.log('x1', this.x1);
   console.log('x2', this.x2);
   functionPlot({
     target: root,
     yAxis: { domain: [this.y1, this.y2] },
     xAxis: { domain: [this.x1, this.x2] },
     tip: {
       renderer: function() {}
     },
     grid: true,
     data: this.data
   })

   // this.x1 = 0;
   // this.x2 = 0;
   // this.y1 = 0;
   // this.y2 = 0;
  }


}
