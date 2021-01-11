import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';

@Component({
  selector: 'app-cleansing',
  templateUrl: './cleansing.component.html'
})


export class CleansingComponent implements OnInit {

  public fileToUpload: File = null;
  public query: CleanseParameters = new CleanseParameters(true);

  constructor(
    private http: HttpClient
  ) { 
  }

  ngOnInit(): void {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
}

  submit(body: NgForm){
    const cleanseParams = 
    `{'AutoCleansing' : ${this.query.AutoCleansing}, 
    'FilterData' : ${this.query.FilterData}, 
    'ReplaceMissingValue': ${this.query.ReplaceMissingValue}, 
    'NormalizeData': ${this.query.NormalizeData},
    'RemoveDublicates': ${this.query.RemoveDublicates}}`


    const data = new FormData();
     data.append('file', this.fileToUpload, this.fileToUpload.name)
    this.http.post('http://localhost:3921/api/cleanse', data, 
    {params: new HttpParams().set('cleanseParams', cleanseParams),
    reportProgress: true, 
    observe: 'events',
    headers: new HttpHeaders({
      "cache-control": "no-cache"
    })
   }).subscribe(
      (data) => console.log(data)
  );



}
}


export class CleanseParameters {
  constructor(
  public AutoCleansing: boolean)  {
    this.FilterData = !AutoCleansing;
    this.ReplaceMissingValue = !AutoCleansing;
    this.NormalizeData = !AutoCleansing;
    this.RemoveDublicates = !AutoCleansing;
  }
  public FilterData: boolean;
  public ReplaceMissingValue: boolean;
  public NormalizeData: boolean;
  public RemoveDublicates: boolean;
}

