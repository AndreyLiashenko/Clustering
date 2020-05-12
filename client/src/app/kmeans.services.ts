import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpParams } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import {KmeansModel} from './shared/kmeans/kmeans-model'
import { Centroid } from './shared/kmeans/centroid';
import {GaussResponse} from './shared/gauss/gauss-response';



@Injectable({providedIn: 'root'})
export class KmeansService{

    apiUrl: string;
	constructor(private http: HttpClient) {}

    getPoints( files: any, numberOfClusters: number) : Observable<HttpEvent<KmeansModel>>{

        let fileToUpload = <File>files[0];
        const formData = new FormData();
		formData.append('file', fileToUpload, fileToUpload.name);
        return this.http.post<KmeansModel>('http://localhost:3921/api/kmeans/frontModel',formData, 
        {params: new HttpParams().set('numberOfClusters',`${numberOfClusters}`),
         reportProgress: true, 
         observe: 'events'
        });
    }

    getGaussianParam( centroids: Centroid, axisNumber: number) : Observable<HttpEvent<GaussResponse[]>>{

        return this.http.post<GaussResponse[]>('http://localhost:3921/api/kmeans/getGaussParam',centroids, 
        {params: new HttpParams().set('axisNumber',`${axisNumber}`),
         reportProgress: true, 
         observe: 'events'
        });
    }
    

}
