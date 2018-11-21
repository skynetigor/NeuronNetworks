import { IHiddenLayer } from '../abstract';
import { OutputLayer } from './output-layer';
import { ILayer } from '../../abstract';

export class HiddenLayer extends OutputLayer implements IHiddenLayer {
    nextLayer: ILayer;

    /**
     * HiddenLayer
     */
    constructor(neuronsCount: number, previousLayer: ILayer, index: number) {
        super(neuronsCount, previousLayer, index);
    }
}
