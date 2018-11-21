import { ILayer, ITrainSet } from '../../abstract';

export abstract class AbstractTeacher {
    /**
     * Abstract teacher
     */
    constructor(protected epochsCount: number, protected learningRate: number) {

    }

    public abstract teach(layers: ILayer[], trainSets: ITrainSet[], mse: (errors: number[]) => void): void;
}
