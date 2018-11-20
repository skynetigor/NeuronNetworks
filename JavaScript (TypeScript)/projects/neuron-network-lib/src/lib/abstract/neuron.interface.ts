import { ILayer } from './layer.interface';

export interface INeuron {
    readonly weights: number[];
    readonly index: number;
    currentLayer: ILayer;
    handle(inputs: number[]): number;
}
