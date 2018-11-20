import { IOutputLayer } from '../abstract';
import { Neuron } from '../../neuron';
import { INeuron, ILayer } from '../../abstract';

export class OutputLayer implements IOutputLayer {
    neurons: INeuron[];
    get inputsCount() {
        return this.previousLayer.outputsCount;
    }

    get outputsCount() {
        return this.neurons.length;
    }

    /**
     *
     */
    constructor(neuronsCount: number, public previousLayer: ILayer, public readonly index: number) {
        this.neurons = [];

        for (let i = 0; i < neuronsCount; i++) {
            this.neurons[i] = new Neuron(this.inputsCount, i, this);
        }
    }

    handle(inputs: number[]): number[] {
        return this.neurons.map(n => n.handle(inputs));
    }
}
