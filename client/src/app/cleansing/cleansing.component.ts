import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpParams, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-cleansing',
  templateUrl: './cleansing.component.html'
})


export class CleansingComponent implements OnInit {

  public fileToUpload: File = null;
  public query: CleanseParameters = new CleanseParameters(true);
  public isDataCleaned: boolean = false;
  public dataCleaned: File = null;
  public linkToDownload: any;

  constructor(
    private http: HttpClient
  ) {
  }

  ngOnInit(): void {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  submit(body: NgForm) {
    const cleanseParams =
      `{'AutoCleansing' : ${this.query.AutoCleansing}, 
    'FilterData' : ${this.query.FilterData}, 
    'ReplaceMissingValue': ${this.query.ReplaceMissingValue}, 
    'NormalizeData': ${this.query.NormalizeData},
    'RemoveDublicates': ${this.query.RemoveDublicates}}`


    this.cleanData(cleanseParams)
      .subscribe(
        event => {
          if (event.type === HttpEventType.Response) {
            console.log(event)
            const rows = [
              ["name1", "city1", "some other info"],
              ["name2", "city2", "more info"]
            ];


            let csvContent = "data:text/csv;charset=utf-8,"
              + event.body.map(e => Object.values(e).join(",")).join("\n");
            let encodedUri = encodeURI(csvContent);

            this.dataCleaned = new File([encodedUri], "cleaned_data.csv", {
              type: "text/csv",
            });

            this.linkToDownload = document.createElement("a");
            this.linkToDownload.setAttribute("href", encodedUri);
            this.linkToDownload.setAttribute("download", "cleaned_data.csv");
            document.body.appendChild(this.linkToDownload); // Required for FF
          }
        });
  }

  cleanData(cleanseParams): Observable<HttpEvent<any>> {
    const data = new FormData();
    data.append('file', this.fileToUpload, this.fileToUpload.name)
    return this.http.post('http://localhost:3921/api/cleanse', data,
      {
        params: new HttpParams().set('cleanseParams', cleanseParams),
        reportProgress: true,
        observe: 'events',
        headers: new HttpHeaders({
          "cache-control": "no-cache"
        })
      })
  }

  download() {
    this.linkToDownload.click();
  }
}



export class CleanseParameters {
  constructor(
    public AutoCleansing: boolean) {
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

