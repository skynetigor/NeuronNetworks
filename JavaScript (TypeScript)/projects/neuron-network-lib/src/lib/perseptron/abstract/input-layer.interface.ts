import { ILayer } from '../../abstract';

export interface IInputLayer extends ILayer {
    nextLayer: ILayer;
}
