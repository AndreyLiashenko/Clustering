import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cleansing',
  templateUrl: './cleansing.component.html'
})


export class CleansingComponent implements OnInit {

  public fileToUpload: File = null;
  public query: CleanseParameters = new CleanseParameters(false, false, false);

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
    `{'FilterData' : ${this.query.FilterData}, 
    'ReplaceMissingValue': ${this.query.ReplaceMissingValue}, 
    'NormalizeData': ${this.query.NormalizeData}}`


    const data = new FormData();
     data.append('file', this.fileToUpload, this.fileToUpload.name)
    this.http.post('http://localhost:5000/api/cleanse', data, 
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
  public FilterData: boolean,
  public ReplaceMissingValue: boolean,
  public NormalizeData: boolean)
  { }
}

