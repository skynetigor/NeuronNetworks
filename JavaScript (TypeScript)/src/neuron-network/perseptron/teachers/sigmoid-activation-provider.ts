import { AbstractActivationProvider } from "../../abstract/abstract-activation-provider";

export class SigmoidActivationProvider extends AbstractActivationProvider {
    public activationFunction(x: number): number {
        return 1 / (1 + Math.pow(Math.exp(1), - x));
    }

    public derivative(x: number): number {
        return this.activationFunction(x) * (1 - this.activationFunction(x) );
    }
}