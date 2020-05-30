import { Type } from 'class-transformer';
import {Point} from '../kmeans/point'
import {Centroid} from '../kmeans/centroid'

export class KmeansModel{
    @Type(() => Point)
    listOfPoint: Point[];
    
    centroids : Centroid;
}