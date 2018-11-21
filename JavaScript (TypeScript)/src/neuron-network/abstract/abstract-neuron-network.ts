import { ITrainSet } from './train-set.interface';

export abstract class AbstractNeuronNetwork {
    public abstract getAnswer(inputs: number[]): number[];

    public abstract study(trainSets: ITrainSet[], epochsCount: number, learningRate: number, mse: (errors: number[]) => void): void;
}
