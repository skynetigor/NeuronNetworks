import { Component } from '@angular/core';
import { ITrainSet, PerseptronNeuronNetwork, PerseptronConfig } from 'neuron-network-lib';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'NeuronNetwork';

  error = '';

  /**
   *
   */
  constructor() {
    setTimeout(this.run.bind(this), 1);
  }

  private run() {
    const n = new PerseptronNeuronNetwork(new PerseptronConfig(3, 1000, 1));
    console.log(n);

    const trainsets: ITrainSet[] = [
      { inputs: [1, 1, 1], expected: [1] },
      { inputs: [0, 0, 0], expected: [0] },
      { inputs: [1, 0, 0], expected: [1] },
      { inputs: [0, 0, 1], expected: [0] },
      { inputs: [0, 1, 0], expected: [1] },
      { inputs: [1, 1, 0], expected: [0] },
      { inputs: [1, 0, 1], expected: [1] },
    ];

    n.study(trainsets, 5000, 0.1, (errors) => { this.error = JSON.stringify(errors); });

    trainsets.forEach(tr => {
      console.log(`Inputs = ${JSON.stringify(tr.inputs)}, Actual = ${n.getAnswer(tr.inputs)[0]}, expected ${tr.expected[0]}`);
    });

    console.log(n.getState());
  }
}
