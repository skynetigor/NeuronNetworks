import { AbstractNeuronNetwork, ITrainSet } from "../abstract";
import { PerseptronNeuronNetwork } from "./perseptron-neuron-network";
import { PerseptronConfig } from "./perseptron-config";

export class ImageNeuronNetwork {
    private internalNeuronNetwork: AbstractNeuronNetwork;

    constructor(dimension: [number, number], hiddenLayersConfig: number[], outputsCount: number) {
        const inputsCount = dimension[0] * dimension[1];

        this.internalNeuronNetwork = new PerseptronNeuronNetwork(new PerseptronConfig(inputsCount, ...hiddenLayersConfig, outputsCount));
    }

    public getAnswer(image: number[][]): number[] {
        const toCheck = this.toSingleDimensionArray(image);

        return this.internalNeuronNetwork.getAnswer(toCheck);
    }

    public study(trainSets: { image: number[][], expected: [] }[], epochCount: number, learningRate: number, mse: (errors: number[]) => void) {
        const _trainSets = trainSets.map<ITrainSet>(t => ({ inputs: this.toSingleDimensionArray(t.image), expected: t.expected }));

        this.internalNeuronNetwork.study(_trainSets, epochCount, learningRate, mse);
    }

    private toSingleDimensionArray(array: number[][]) {
        let result: number[] = [];

        array.forEach(v => {
            result = result.concat(v);
        });

        return result;
    }
}