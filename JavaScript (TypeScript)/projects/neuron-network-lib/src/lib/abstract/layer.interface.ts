export interface ILayer {
    readonly index: number;
    readonly inputsCount: number;
    readonly outputsCount: number;
    handle(inputs: number[]): number[];
}
