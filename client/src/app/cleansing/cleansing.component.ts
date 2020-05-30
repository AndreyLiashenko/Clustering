import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cleansing',
  templateUrl: './cleansing.component.html',
  styleUrls: ['./cleansing.component.css'],
})
export class CleansingComponent implements OnInit {

  fileToUpload: File = null;


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
    let headers = new HttpHeaders({
      'Content-Type': 'multipart/form-data'
    })
    let options = {
      headers: headers
    }

    const data = new FormData();
     data.append('file', this.fileToUpload, this.fileToUpload.name)
    this.http.post('http://localhost:5000/api/cleanse', data, 
    {params: new HttpParams().set('cleanseParams',`{'FilterData' : false, 'ReplaceMissingValue': true, 'NormalizeData': true}`),
    reportProgress: true, 
    observe: 'events'
   }).subscribe(
      (data) => console.log(data)
  );



}
}

