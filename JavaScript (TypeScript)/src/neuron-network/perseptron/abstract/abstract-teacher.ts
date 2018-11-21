import { ILayer, ITrainSet } from '../../abstract';
import { AbstractActivationProvider } from '../../abstract/abstract-activation-provider';

export abstract class AbstractTeacher {
    /**
     * Abstract teacher
     */
    constructor(protected activationProvider: AbstractActivationProvider ,protected epochsCount: number, protected learningRate: number) {

    }

    public abstract teach(layers: ILayer[], trainSets: ITrainSet[], mse: (errors: number[]) => void): void;
}
