import Puzzle from '../../types/AbstractPuzzle';

interface PointOnMap {
  character: string,
  x: number,
  y: number,
}


export default class ConcretePuzzle extends Puzzle {
  map: PointOnMap[] = [];
  mapWidth = 0;
  mapHeight = 0

  public solveFirst(): string {
    const splitInput = this.input.split('\r\n');
    this.mapHeight = splitInput.length;
    this.mapWidth = splitInput[0].length + 1;

    for (let y = 0; y < this.mapHeight; y++) {
      for (let x = 0; x < this.mapWidth; x++) {
        const character = splitInput[y][x];
        this.map.push({
          character: character,
          x: x,
          y: y
        });
      }
    }

    this.mapHeight--;
    this.mapWidth--;

    var numberOfMoves = 1;
    var turnCount = 0;

    //this.displayMap();
    while (numberOfMoves > 0) {
      numberOfMoves = 0;

      this.map.filter(mov => mov.character === '>' && this.getNode(mov.y, mov.x + 1).character === '.')
        .forEach(mov => {
          mov.character = '.';
          this.getNode(mov.y, mov.x + 1).character = '>'
          numberOfMoves++;
        });

      this.map.filter(mov => mov.character === 'v' && this.getNode(mov.y + 1, mov.x).character === '.')
        .forEach(mov => {
          mov.character = '.';
          this.getNode(mov.y + 1, mov.x).character = 'v';
          numberOfMoves++;
        });

      turnCount++;
      //this.displayMap();
    }

    return turnCount.toString();
  }

  private getNode(y: number, x: number): PointOnMap {
    return this.map.find(search => search.x == x % this.mapWidth && search.y == (y % this.mapHeight));
  }

  private displayMap() {
    var width = Math.max.apply(Math, this.map.map(aa => aa.x));
    var height = Math.max.apply(Math, this.map.map(aa => aa.y));

    for (let y = 0; y < height; y++) {
      var line = '';
      for (let x = 0; x < width; x++) {
        line += this.getNode(y, x).character;
      }
      console.log(line);
    }

    console.log('---------');
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
}
