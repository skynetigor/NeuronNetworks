import { PerseptronNeuronNetwork } from '../neuron-network/perseptron/perseptron-neuron-network';
import { PerseptronConfig } from '../neuron-network/perseptron/perseptron-config';
import { ITrainSet } from '../neuron-network/abstract';

function run() {
    const n = new PerseptronNeuronNetwork(new PerseptronConfig(3, 1000, 1));
    console.log(n);

    const trainsets: ITrainSet[] = [
      { inputs: [1, 1, 1], expected: [1] },
      { inputs: [0, 0, 0], expected: [0] },
      { inputs: [1, 0, 0], expected: [1] },
      { inputs: [0, 0, 1], expected: [0] },
      { inputs: [0, 1, 0], expected: [1] },
    //   { inputs: [1, 1, 0], expected: [0] },
    //   { inputs: [1, 0, 1], expected: [1] },
    ];

    let error: any;
    n.study(trainsets, 10000, 0.1, (errors) => { console.clear(); console.log(JSON.stringify(errors)); });

    trainsets.forEach(tr => {
      console.log(`Inputs = ${JSON.stringify(tr.inputs)}, Actual = ${n.getAnswer(tr.inputs)[0]}, expected ${tr.expected[0]}`);
    });
}

run();