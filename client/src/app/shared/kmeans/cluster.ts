//import { Type } from 'class-transformer';
import {Point} from '../kmeans/point'

export class Cluster{
    clusterId: number;
    rgbColor: string;

    // @Type(() => Point)
	listOfPoint: Point[];
}