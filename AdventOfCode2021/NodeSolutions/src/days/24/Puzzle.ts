import Puzzle from '../../types/AbstractPuzzle';
import { ALU } from './Alu';

export default class ArithmeticLogicUnitPuzzle extends Puzzle {
  public solveFirst(): string {
    var lines = this.input.split('\r\n');
    var alu = new ALU(lines);
    var validNumbers = [];

    while (validNumbers.length < 20) {
      var input = '0';
      input = this.randomNumber(14);


      alu.runMethod(input);
      if (alu.variables.z == 0 && parseInt(input) < 11419161313147) {
        console.log(input);
        validNumbers.push(input)
      }
    }

    return validNumbers.filter((v, i, a) => a.indexOf(v) === i).sort()[0];

    // var checkNumber = 99999999999999;
    // var xx = 0;
    // while (true) {
    //   xx++;
    //   if (xx % 1000000 == 0) {
    //     console.log('checking ', checkNumber);
    //   }

    //   if (checkNumber.toString().indexOf('0') > 0) {
    //     checkNumber--;
    //     continue;
    //   }

    //   var alu = new ALU(checkNumber.toString());
    //   for (let i = 0; i < lines.length; i++) {
    //     alu.runMethod(lines[i]);
    //   }

    //   if (alu.z == 0) {
    //     break;
    //   }

    //   checkNumber--;
    // }

    return "test";
  }

  public getFirstExpectedResult(): string {
    // RETURN EXPECTED SOLUTION FOR TEST 1;
    return 'day 1 solution 1';
  }

  public solveSecond(): string {
    // WRITE SOLUTION FOR TEST 2
    return 'day 1 solution 2';
  }

  public getSecondExpectedResult(): string {
    // RETURN EXPECTED SOLUTION FOR TEST 2;
    return 'day 1 solution 2';
  }

  private randomNumber(length: number): string {
    var result = '114';
    var firstCharacter = '3';
    var secondCharacter = '789';
    var characters = '123456789';
    var charactersLength = characters.length;

    // while (isNaN(parseInt(result)) || parseInt(result) <= 26969794979198) {
    //   result = '';
    //   result += '269';
    //   for (var i = 0; i < length - 6; i++) {
    //     result += characters.charAt(Math.floor(Math.random() * charactersLength));
    //   }

    //   result += '198';
    // }

    // result += firstCharacter.charAt(Math.floor(Math.random() * firstCharacter.length));
    // if (result === '2') {
    //   result += secondCharacter.charAt(Math.floor(Math.random() * secondCharacter.length));
    // }
    // else {
    //   result += characters.charAt(Math.floor(Math.random() * characters.length));
    // }

    for (var i = 0; i < length - 6; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }

    result += '147';

    return result;
  }
}
