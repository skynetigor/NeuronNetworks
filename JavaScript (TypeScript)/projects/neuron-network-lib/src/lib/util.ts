export class Utils {
    public static throughSigmoid(x: number) {
        return 1 / (1 + Math.pow(Math.exp(1), - x));
    }

    public static weightsDelta(x: number) {
        return Utils.throughSigmoid(x) * (1 - Utils.throughSigmoid(x) );
    }

    public static randomInteger(min: number, max: number) {
        let rand = min - 0.5 + Math.random() * (max - min + 1);
        rand = Math.round(rand);
        return rand;
    }
}
