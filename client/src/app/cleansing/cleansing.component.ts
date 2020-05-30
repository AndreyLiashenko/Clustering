import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import {HttpClient} from '@angular/common/http';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cleansing',
  templateUrl: './cleansing.component.html',
  styleUrls: ['./cleansing.component.css'],
})
export class CleansingComponent implements OnInit {

  FilterData: boolean;
  ReplaceMissingValue: boolean;
  NormalizeData: boolean;


  constructor(
    private http: HttpClient
  ) { 

  }

  ngOnInit(): void {
  }

  submit(form: NgForm){
    const body = {cleanseParams:{'FilterData' : false,  'ReplaceMissingValue': true, 'NormalizeData': true} }
    this.http.post('http://localhost:5000/api/cleanse', body).subscribe(
      (data) => console.log(data)
  );
}
}

