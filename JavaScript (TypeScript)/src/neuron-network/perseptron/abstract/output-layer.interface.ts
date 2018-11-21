import { ILayer, INeuron } from '../../abstract';

export interface IOutputLayer extends ILayer {
    neurons: INeuron[];
    previousLayer: ILayer;
}
