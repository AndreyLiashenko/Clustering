import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import {KmeansModel} from '../shared/kmeans/kmeans-model'
import {KmeansService} from '../kmeans.services';
import { HttpEventType } from '@angular/common/http';
import {Algorithms} from '../shared/algorithms-model'

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css'],
  providers: [KmeansService]
})
export class UploadComponent implements OnInit {
  public message: string;
  public progress: number;
  public numberOfCluster: number;
  public onChangeAlgorithmProperty: Algorithms;
  @Input() enumsOfAlgorithms: Algorithms;
  algorithms = Algorithms;
  @Output() public onUploadFinished = new EventEmitter();
  @Output() public numberOfClusterEmit = new EventEmitter();

  model: KmeansModel
  constructor(private kmeansService: KmeansService) { }

  ngOnInit(): void {
  }

  getPoints(files: any){
    this.kmeansService.getPoints(files, this.numberOfCluster, this.onChangeAlgorithmProperty)
    .subscribe(event => {
      if(event.type === HttpEventType.UploadProgress){
        this.progress = Math.round((100 * event.loaded) / event.total);
      }

      else if(event.type === HttpEventType.Response){
        this.message = "Upload success.";
        console.log('body', event.body);
        this.onUploadFinished.emit(event.body);
      }
    })

  }

  emitNumberOfCluster(){
      this.numberOfClusterEmit.emit(this.numberOfCluster);
  }

  public onChangeAlgorithm(event){
    const value = event.target.value;
    console.log('value', value);
    this.onChangeAlgorithmProperty = value;
  }
}
