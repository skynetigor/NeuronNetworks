import { Observable } from 'rxjs';

import { PerseptronConfig } from './perseptron-config';

import { HiddenLayer, InputLayer, OutputLayer } from './Layers';
import { PerseptronTeacher } from './teachers';
import { ILayer, AbstractNeuronNetwork, ITrainSet } from '../abstract';
import { IInputLayer, IOutputLayer } from './abstract';
import { Utils } from 'neuron-network-lib/util';
import { NeuronNetworkStateManager } from './state-saver';

export class PerseptronNeuronNetwork extends AbstractNeuronNetwork {
    private layers: ILayer[];

    /**
     * Inititalizes new instance
     */
    constructor(perseptronConfig: PerseptronConfig) {
        super();
        this.layers = [];
        this.layers[0] = new InputLayer(perseptronConfig.neuronsPerLayer[0], 0);

        if (perseptronConfig.neuronsPerLayer.length > 1) {
            for (let i = 1; i < perseptronConfig.neuronsPerLayer.length; i++) {
                const previousLayer = this.layers[i - 1];

                if (i === perseptronConfig.neuronsPerLayer.length - 1) {
                    this.layers[i] = new OutputLayer(perseptronConfig.neuronsPerLayer[i], previousLayer, i);
                } else {
                    this.layers[i] = new HiddenLayer(perseptronConfig.neuronsPerLayer[i], previousLayer, i);
                }
            }

            for (let i = 0; i < this.layers.length - 1; i++) {
                const layer: IInputLayer = <any>this.layers[i];
                layer.nextLayer = this.layers[i + 1];
            }
        }
    }

    public getAnswer(inputs: number[]): number[] {

        let output = inputs;
        for (let i = 1; i < this.layers.length; i++) {
            output = this.layers[i].handle(output).map(Utils.throughSigmoid);
        }

        return output;
    }

    public study(trainSets: ITrainSet[], epochsCount: number, learningRate: number, mse: (errors: number[]) => void): Observable<number[]> {
        const teacher = new PerseptronTeacher(epochsCount, learningRate);

        return teacher.teach(this.layers, trainSets, mse);
    }

    public getState() {
        return new NeuronNetworkStateManager().getState(this.layers);
    }
}
