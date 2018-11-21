export abstract class AbstractActivationProvider {
    public abstract activationFunction(x: number): number;
    public abstract derivative(x: number): number;
}