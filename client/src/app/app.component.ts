import { Component, OnInit, Input } from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser'

import {KmeansModel} from './shared/kmeans/kmeans-model'
import {Graph} from './shared/plotly-clustering-model/graph'
import {Cluster} from './shared/kmeans/cluster';
import {PlotlyClusteringService} from './plotly-clustering.services';
import { Centroid } from './shared/kmeans/centroid';
import {Algorithms} from './shared/algorithms-model'
import * as jsonOfRules from './rules-example.json';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  //styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    graph: Graph;
    isOneCluster: boolean = false;
    numberOfCluster: number;
    listOfCluster: Cluster[] = [];
    centroids : Centroid;

  constructor(private plotlyClusteringService : PlotlyClusteringService, private sanitizer: DomSanitizer ){
      this.centroids = new Centroid();
  }

  ngOnInit(){
      this.graph = new Graph();
      this.graph =  this.plotlyClusteringService.oneClustering('rgb(23, 190, 207)',[{x: 1, y: 7, z: 3}]);
  }

  }

  ngOnInit(){
     
  }
}
