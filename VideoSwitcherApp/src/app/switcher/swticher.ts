export interface Switcher {
    id: string;
    productName: string;
    inputs: SwitcherInput[];
    currentProgramInput: SwitcherInput;
}

export interface SwitcherInput {
    id: number;
    name: string;
}