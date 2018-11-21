import { ILayer } from '../../abstract';
import { IInputLayer } from '../abstract';

export class InputLayer implements IInputLayer {
    nextLayer: ILayer;
    outputsCount: number;

    /**
     * Input layer
     */
    constructor(public readonly inputsCount: number, public readonly index: number) {
        this.outputsCount = inputsCount;
    }

    handle(inputs: number[]): number[] {
        return inputs;
    }
}
