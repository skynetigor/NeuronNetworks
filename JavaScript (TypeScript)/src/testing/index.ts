import { PerseptronNeuronNetwork } from '../neuron-network/perseptron/perseptron-neuron-network';
import { PerseptronConfig } from '../neuron-network/perseptron/perseptron-config';
import { ITrainSet } from '../neuron-network/abstract';
import { ImageNeuronNetwork } from '../neuron-network/perseptron/image-neuron-network';

function runSimplePerseptron() {
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

function runArray() {
  const n = new ImageNeuronNetwork([10, 10], [3, 4, 5], 1);

  const ass = n.getAnswer(generateTwoDimensionArray());

}

function generateTwoDimensionArray(): number[][] {
  const result: number[][] = [];

  for (let i = 0; i < 10; i++) {
    const t: number[] = [];
    for (let j = 0; j < 10; j++) {
      t.push(0);
    }
    result.push(t);
  }

  return result;
}

// runSimplePerseptron();
runArray();