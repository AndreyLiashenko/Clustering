import { Component, OnInit, Input } from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser'
import { HttpEventType } from '@angular/common/http';
import {KmeansService} from '../kmeans.services';
import {GaussResponse} from '../shared/gauss/gauss-response';
import {PlotFunctionModel} from '../shared/plot-function-model';
import * as jsonOfRules from '../rules-example.json';


@Component({
  selector: 'app-function-plot',
  templateUrl: './function-plot.component.html',
  styleUrls: ['./function-plot.component.css']
})
export class FunctionPlotComponent implements OnInit {
  
  @Input() centroids: Centroid;
  gaussResponse: GaussResponse[] =[];
  data: PlotFunctionModel[] = [];
  x1: number;
  x2: number;
  y1: number = -0.1;
  y2: number = 1;
  axis: number;
  downloadJsonHref: any;
  downloadLink: boolean = false;

  constructor(private kmeansService: KmeansService, private sanitizer: DomSanitizer) { 
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
        console.log('centroids', this.centroids);
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

//   ngOnInit(): void {
//   }

   

    // this.x1 = 0;
    // this.x2 = 0;
    // this.y1 = 0;
    // this.y2 = 0;
   }

   generateDownloadJsonUri() : any {
    var theJSON = JSON.stringify(jsonOfRules);
    console.log("DOWNLOAD");
    var uri = this.sanitizer.bypassSecurityTrustUrl("data:text/json;charset=UTF-8," + encodeURIComponent(theJSON));
    this.downloadJsonHref = uri;
    this.downloadLink = true;
  }
}
