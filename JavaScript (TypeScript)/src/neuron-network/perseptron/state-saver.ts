import { InputLayer } from './Layers';

export class NeuronNetworkStateManager {
    public getState(array: any[]) {
        const result = [];


        for (let i = 1; i < array.length; i++) {
            const v: any = array[i];

            

            result.push(v.neurons.map((t: any) => t.weights));
        }

        return result;
    }

    public loadFromState(state: { classname: string, neurons: number[][] }[]) {
        const layers = [new InputLayer(0, 0)];
        state.forEach(v => {

        });
    }


}
