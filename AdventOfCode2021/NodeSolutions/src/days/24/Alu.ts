export interface Command {
  fullCommand: string;
  runCommand: Function;
}

export class ALU {
  public variables = {
    x: 0,
    y: 0,
    w: 0,
    z: 0,
  }

  public input: string;

  private commands: Command[];

  constructor(commands: string[]) {
    this.commands = commands.map(x => {
      let method = x.split(' ')[0];
      let variable1 = x.split(' ')[1];
      let variable2 = x.split(' ')[2];

      let returnCommand: Command = {
        fullCommand: x,
        runCommand: () => { }
      };

      switch (method) {
        case 'inp':
          returnCommand.runCommand = () => {
            (this.variables as any)[variable1] = parseInt(this.input[0]);
            this.input = this.input.substring(1);
          }
          break;

        case 'add':
          let addNumber = parseInt(variable2);
          if (isNaN(addNumber)) {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] + (this.variables as any)[variable2];
            };
          }
          else {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] + addNumber;
            };
          }
          break;

        case 'mul':
          let mulNumber = parseInt(variable2);
          if (isNaN(mulNumber)) {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] * (this.variables as any)[variable2];
            };
          }
          else {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] * mulNumber;
            };
          }

          break;

        case 'div':
          let divNumber = parseInt(variable2);
          if (isNaN(divNumber)) {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = Math.floor((this.variables as any)[variable1] / (this.variables as any)[variable2]);
            };
          }
          else {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = Math.floor((this.variables as any)[variable1] / divNumber);
            };
          }
          break;

        case 'mod':
          let modNumber = parseInt(variable2);
          if (isNaN(modNumber)) {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] % (this.variables as any)[variable2];
            };
          }
          else {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] % modNumber;
            };
          }
          break;

        case 'eql':
          let eqNumber = parseInt(variable2);
          if (isNaN(eqNumber)) {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] == (this.variables as any)[variable2] ? 1 : 0;
            };
          }
          else {
            returnCommand.runCommand = () => {
              (this.variables as any)[variable1] = (this.variables as any)[variable1] == eqNumber ? 1 : 0;
            };
          }
          break;
        default:
          break;
      }

      return returnCommand;
    });
  }

  runMethod(input: string) {
    this.input = input;
    this.variables = {
      x: 0,
      y: 0,
      w: 0,
      z: 0,
    };

    this.commands.forEach(x => {
      x.runCommand();
    });
  }
}