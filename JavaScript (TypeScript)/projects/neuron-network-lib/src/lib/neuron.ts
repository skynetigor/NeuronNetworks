import { ILayer, INeuron } from './abstract';
import { Utils } from './util';

export class Neuron implements INeuron {
    public readonly weights: number[];

    public currentLayer: ILayer;

    public readonly index: number;

    private initializeWeights(weightLength: number): number[] {
        const weights = [];

        for (let i = 0; i < weightLength; i++) {
            weights[i] = 1 / Utils.randomInteger(2, 50);
        }

        return weights;
    }

    /**
     * Neuron
     */
    constructor(weightLength: number, index: number, currentLayer: ILayer) {
        this.weights = this.initializeWeights(weightLength);
        this.index = index;
        currentLayer = currentLayer;
    }

    handle(inputs: number[]): number {
        let power = 0;

        for (let i = 0; i < this.weights.length; i++) {
            power += this.weights[i] * inputs[i];
        }

        return power;
    }
}
