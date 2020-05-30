import { Component, OnInit } from '@angular/core';
import {PlotlyClusteringService} from '../plotly-clustering.services';
import {KmeansModel} from '../shared/kmeans/kmeans-model'
import {Graph} from '../shared/plotly-clustering-model/graph'
import {Cluster} from '../shared/kmeans/cluster';

import { Centroid } from '../shared/kmeans/centroid';

@Component({
  selector: 'app-clustering',
  templateUrl: './clustering.component.html',
  styleUrls: ['./clustering.component.css']
})
export class ClusteringComponent implements OnInit {

  graph: Graph;
    isOneCluster: boolean = false;
    numberOfCluster: number;
    listOfCluster: Cluster[] = [];
    centroids : Centroid;

  constructor(
    private plotlyClusteringService : PlotlyClusteringService
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

}
