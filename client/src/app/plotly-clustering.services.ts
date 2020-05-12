import { Injectable } from '@angular/core';

import {Graph} from './shared/plotly-clustering-model/graph'
import {Cluster} from './shared/kmeans/cluster';

@Injectable({providedIn: 'root'})
export class PlotlyClusteringService{

    data:any;
    layout: any;
    graph: Graph;
    listOfCluster: Cluster[] = [];
    constructor(){
        this.graph = new Graph();
        this.initialLayout();
    }

    unpack(rows, key) {
        return rows.map(function (row) { return row[key]; });
    }

     oneClustering(pointColor: string, listOfPoint: any) :Graph {
        this.data =  [
            {
              x: this.unpack(listOfPoint, 'x'),
              y: this.unpack(listOfPoint, 'y'),
              z: this.unpack(listOfPoint, 'z'),
              mode: 'markers',
              type: 'scatter3d',
              marker: {
                color: pointColor,
                size: 2
              }
          },{
              alphahull: 7,
              opacity: 0.1,
              type: 'mesh3d',
              x: this.unpack(listOfPoint, 'x'),
              y: this.unpack(listOfPoint, 'y'),
              z: this.unpack(listOfPoint, 'z')
          } ];

            this.graph.data = this.data;
            this.graph.layout = this.layout;
            return this.graph;
      }

      initialLayout(){

        this.layout = {
                autosize: true,
                height: 500,
                scene: {
                    aspectratio: {
                        x: 1,
                        y: 1,
                        z: 1
                    },
                    camera: {
                        center: {
                            x: 0,
                            y: 0,
                            z: 0
                        },
                        eye: {
                            x: 1.25,
                            y: 1.25,
                            z: 1.25
                        },
                        up: {
                            x: 0,
                            y: 0,
                            z: 1
                        }
                    },
                    xaxis: {
                        type: 'linear',
                        zeroline: false
                    },
                    yaxis: {
                        type: 'linear',
                        zeroline: false
                    },
                    zaxis: {
                        type: 'linear',
                        zeroline: false
                    }
                },
                title: '3d point clustering',
                width: 1000};
  }

 allClustering(listOfPoint: Cluster[]) :Graph {
    for(let item of listOfPoint){
        let data1 = {
          x: this.unpack(item.listOfPoint, 'x'),
          y: this.unpack(item.listOfPoint, 'y'),
          z: this.unpack(item.listOfPoint, 'z'),
          mode: 'markers',
          type: 'scatter3d',
          marker: {
            color: item.rgbColor,
            size: 2
          }
        };

        this.data.push(data1);
        let data2 = {
            alphahull: 7,
            opacity: 0.1,
            type: 'mesh3d',
            x: this.unpack(item.listOfPoint, 'x'),
            y: this.unpack(item.listOfPoint, 'y'),
            z: this.unpack(item.listOfPoint, 'z')
        }

        this.data.push(data2);
    }

        this.graph.data = this.data;
        console.log('data', this.graph.data);
        this.graph.layout = this.layout;
        return this.graph;
  }

}