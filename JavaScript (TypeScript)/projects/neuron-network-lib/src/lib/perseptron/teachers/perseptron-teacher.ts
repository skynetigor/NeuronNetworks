import { Observable, Subject } from 'rxjs';

import { ILayer, ITrainSet, INeuron } from '../../abstract';
import { AbstractTeacher, IOutputLayer } from '../abstract';
import { Utils } from 'neuron-network-lib/util';

export class PerseptronTeacher extends AbstractTeacher {
    public teach(layers: ILayer[], trainSets: ITrainSet[], mse: (errors: number[]) => void): Observable<number[]> {
        const mseSubject = new Subject<number[]>();
        const outputLayer = layers[layers.length - 1];

        for (let epochIndex = 0; epochIndex < this.epochsCount; epochIndex++) {
            trainSets.forEach(trainSet => {
                const inputs = trainSet.inputs;
                const expected = trainSet.expected;

                const inputsPerLayer: number[][] = [];
                const outputsPerLayer: number[][] = [];

                for (let layerIndex = 0; layerIndex < layers.length; layerIndex++) {
                    const layer = layers[layerIndex];

                    const inputPerLayer = layerIndex === 0 ? inputs : outputsPerLayer[layerIndex - 1];

                    inputsPerLayer[layerIndex] = inputPerLayer;
                    outputsPerLayer[layerIndex] = layerIndex === 0 ? inputPerLayer : layer.handle(inputPerLayer).map(Utils.throughSigmoid);
                }

                let errors = [];

                for (let neuronIndex = 0; neuronIndex < outputLayer.outputsCount; neuronIndex++) {
                    errors[neuronIndex] = outputsPerLayer[outputLayer.index][neuronIndex] - expected[neuronIndex];
                }

                if (mse instanceof Function) {
                    mse(errors);
                }

                for (let i = layers.length - 1; i >= 0; i--) {
                    const l = layers[i];
                    if (l.index !== 0) {
                       errors = this.teachLayer(<any>l, layers[i - 1], errors, inputsPerLayer, outputsPerLayer);
                    }
                }

            });
        }

        return mseSubject.asObservable();
    }

    private teachLayer(currentLayer: IOutputLayer, prev: ILayer, errors: number[], inputsPerLayer: number[][], outputsPerLayer: number[][]) {
        const wd = [];

        for (let neuronIndex = 0; neuronIndex < currentLayer.outputsCount; neuronIndex++) {
            const neuron: INeuron = currentLayer.neurons[neuronIndex];
            const error = errors[neuronIndex];
            const weightsDelta = error * (Utils.weightsDelta(error));
            wd[neuronIndex] = weightsDelta;

            for (let weightIndex = 0; weightIndex < neuron.weights.length; weightIndex++) {
                neuron.weights[weightIndex] = neuron.weights[weightIndex] - outputsPerLayer[prev.index][weightIndex] * weightsDelta * this.learningRate;
            }
        }

        if (prev.index !== 0) {
            const outputErrors = [];

            for (let i = 0; i < prev.outputsCount; i++) {
                for (let j = 0; j < currentLayer.outputsCount; j++) {
                    let err = outputErrors[i] || 0;
                    err = err + currentLayer.neurons[j].weights[i] * wd[j];
                    outputErrors[i] = err;
                }
            }

            return outputErrors;
        }
    }

}
