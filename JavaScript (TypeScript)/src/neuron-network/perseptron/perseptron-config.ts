export class PerseptronConfig {

    constructor(...neuronsPerLayer: number[]) {
        this.neuronsPerLayer = neuronsPerLayer;
    }

    public readonly neuronsPerLayer: number[];
}
