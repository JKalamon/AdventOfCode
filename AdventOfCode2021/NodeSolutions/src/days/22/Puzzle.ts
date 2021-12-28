import Puzzle from '../../types/AbstractPuzzle';

class LineSegment {

  static fromString(inputString: string) {
    var numbers = inputString.replace('x=', '').replace('y=', '').replace('z=', '').split('..');
    return new LineSegment(parseInt(numbers[0]), parseInt(numbers[1]));
  }

  constructor(public p1: number, public p2: number) {
    if (p1 > p2) {
      let temp = p2;
      this.p2 = this.p1;
      this.p1 = temp;
    }
  }

  public intersect(line: LineSegment) {
    return (this.p1 <= line.p1 && line.p1 <= this.p2) || (this.p1 <= line.p2 && line.p2 <= this.p2) || (line.p1 <= this.p1 && this.p1 <= line.p2) || (line.p1 <= this.p2 && this.p2 <= line.p2);
  }

  public equals(line: LineSegment) {
    return this.p1 === line.p1 && this.p2 === line.p2;
  }

  ////Take max from both
  public combine(line: LineSegment) {
    var p1 = line.p1 < this.p1 ? line.p1 : this.p1;
    var p2 = line.p2 > this.p2 ? line.p2 : this.p2;
    return new LineSegment(p1, p2);
  }

  public substract(line: LineSegment): LineSegment[] {
    if (!this.intersect(line)) {
      //return [this];
    }

    var returnArray = [];
    if (this.p1 < line.p1) {
      returnArray.push(new LineSegment(this.p1, line.p1 - 1));
    }

    if (line.p2 < this.p2) {
      returnArray.push(new LineSegment(line.p2 + 1, this.p2));
    }

    return returnArray;
  }

  public contains(line: LineSegment) {
    return this.p1 <= line.p1 && line.p1 <= this.p2 && this.p1 <= line.p2 && line.p2 <= this.p2;
  }

  public get length() {
    return this.p2 - this.p1 + 1;
    return this.p2 - this.p1;
    if (this.p2 - this.p1 >= 2) {

    }
  }
}

class SmallCube {
  constructor(public x: number, public y: number, public z: number) { }

  public get id() {
    return `${this.x}${this.y}${this.z}`;
  }

  public equals(cube: SmallCube) {
    return cube.x === this.x && this.y === cube.y && this.z === cube.z;
  }
}

class Cuboid {
  constructor(public x: LineSegment, public y: LineSegment, public z: LineSegment) {
  }

  public intersect(cube: Cuboid) {
    return this.x.intersect(cube.x) && this.y.intersect(cube.y) && this.z.intersect(cube.z);
  }

  public addCube(cube: Cuboid): Cuboid[] {
    if (!this.intersect(cube)) {
      return [this, cube];
    }

    if (this.containsFully(cube)) {
      return [this];
    }

    if (cube.containsFully(this)) {
      return [cube];
    }

    if (this.numberOfTheSameLineSegments(cube) >= 2) {
      return [new Cuboid(this.x.combine(cube.x), this.y.combine(cube.y), this.z.combine(cube.z))];
    }


    var result: Cuboid[] = [];
    var deltaX = cube.x.substract(this.x);
    var xSubstracted = cube.x;
    deltaX.forEach(xSubes => {
      result.push(new Cuboid(xSubes, cube.y, cube.z));
      xSubstracted = xSubstracted.substract(xSubes)[0];
    });

    var deltaY = cube.y.substract(this.y);
    var ySubstracted = cube.y;
    deltaY.forEach(ySubs => {
      result.push(new Cuboid(xSubstracted, ySubs, cube.z));
      ySubstracted = ySubstracted.substract(ySubs)[0];
    });

    var deltaZ = cube.z.substract(this.z);
    deltaZ.forEach(zSubs => {
      result.push(new Cuboid(xSubstracted, ySubstracted, zSubs));
    });

    return result;
  }

  public substractCube(cube: Cuboid): Cuboid[] {
    if (!this.intersect(cube)) {
      return [this];
    }

    if (cube.containsFully(this)) {
      return [];
    }

    var result: Cuboid[] = [];
    var deltaX = this.x.substract(cube.x);
    var xSubstracted = this.x;
    deltaX.forEach(xSubes => {
      result.push(new Cuboid(xSubes, this.y, this.z));
      xSubstracted = xSubstracted.substract(xSubes)[0];
    });

    var deltaY = this.y.substract(cube.y);
    var ySubstracted = this.y;
    deltaY.forEach(ySubs => {
      result.push(new Cuboid(xSubstracted, ySubs, this.z));
      ySubstracted = ySubstracted.substract(ySubs)[0];
    });

    var deltaZ = this.z.substract(cube.z);
    deltaZ.forEach(zSubs => {
      result.push(new Cuboid(xSubstracted, ySubstracted, zSubs));
    });

    return result;
  }

  public get numberOfCubes() {
    return this.x.length * this.y.length * this.z.length;
  }

  public toString() {
    return `CUBES: ${this.numberOfCubes} x=${this.x.p1}..${this.x.p2},y=${this.y.p1}..${this.y.p2},z=${this.z.p1}..${this.z.p2}`;
  }

  public smallCubes() {
    var result = [];
    for (let x1 = this.x.p1; x1 <= this.x.p2; x1++)
      for (let y1 = this.y.p1; y1 <= this.y.p2; y1++)
        for (let z1 = this.z.p1; z1 <= this.z.p2; z1++)
          result.push(new SmallCube(x1, y1, z1));


    return result;
  }

  public containsFully(cube: Cuboid) {
    return this.x.contains(cube.x) && this.y.contains(cube.y) && this.z.contains(cube.z);
  }

  private numberOfTheSameLineSegments(cube: Cuboid) {
    var theSameLineSegmentCount = 0;
    if (this.x.equals(cube.x)) {
      theSameLineSegmentCount++;
    }

    if (this.y.equals(cube.y)) {
      theSameLineSegmentCount++;
    }

    if (this.y.equals(cube.z)) {
      theSameLineSegmentCount++;
    }

    return theSameLineSegmentCount;
  }
}

export default class ConcretePuzzle extends Puzzle {

  public solveFirst(): string {

    var cubes: Cuboid[] = [];
    var mainBoard = new Cuboid(new LineSegment(-50, 50), new LineSegment(-50, 50), new LineSegment(-50, 50));

    var commands = this.input.split('\r\n');
    var smallCubes: string[] = [];
    commands.forEach(command => {
      console.log(`processing command ${command}`);
      var dimensions = command.split(' ')[1].split(',');
      var cuboid = new Cuboid(LineSegment.fromString(dimensions[0]), LineSegment.fromString(dimensions[1]), LineSegment.fromString(dimensions[2]));
      if (!mainBoard.intersect(cuboid)) {
        console.log('skipped');
        return;
      }

      if (command.split(' ')[0] === 'on') {
        cubes.push(cuboid);
      }
      else {
        while (cubes.some(x => cuboid.intersect(x))) {
          var cubeToProcess = cubes.find(x => cuboid.intersect(x));
          var newCubes = cubeToProcess.substractCube(cuboid);
          cubes.splice(cubes.indexOf(cubeToProcess), 1);
          cubes = cubes.concat(newCubes);
        }
      }

      while (cubes.some(x => cubes.some(y => y !== x && x.intersect(y)))) {
        var cubeToProcess = cubes.find(x => cubes.some(y => y !== x && x.intersect(y)));
        var cubeToAdd = cubes.find(x => x != cubeToProcess && cubeToProcess.intersect(x));
        var newCubes = cubeToProcess.addCube(cubeToAdd);
        cubes.splice(cubes.indexOf(cubeToAdd), 1);
        if (newCubes[0].containsFully(cubeToProcess)) {
          cubes.splice(cubes.indexOf(cubeToProcess), 1);
        }
        cubes = cubes.concat(newCubes);
      }
    });


    var count = 0;
    cubes.forEach(x => {
      console.log(x.toString());
      count += x.numberOfCubes;
    });

    // WRITE SOLUTION FOR TEST 1
    return count.toString();
  }

  public getFirstExpectedResult(): string {
    // RETURN EXPECTED SOLUTION FOR TEST 1;
    return 'day 1 solution 1';
  }

  public solveSecond(): string {
    var cubes: Cuboid[] = [];
    //var mainBoard = new Cuboid(new LineSegment(-50, 50), new LineSegment(-50, 50), new LineSegment(-50, 50));

    var commands = this.input.split('\r\n');
    var smallCubes: string[] = [];
    commands.forEach(command => {
      console.log(`processing command ${command}`);
      var dimensions = command.split(' ')[1].split(',');
      var cuboid = new Cuboid(LineSegment.fromString(dimensions[0]), LineSegment.fromString(dimensions[1]), LineSegment.fromString(dimensions[2]));

      if (command.split(' ')[0] === 'on') {
        cubes.push(cuboid);
      }
      else {
        while (cubes.some(x => cuboid.intersect(x))) {
          var cubeToProcess = cubes.find(x => cuboid.intersect(x));
          var newCubes = cubeToProcess.substractCube(cuboid);
          cubes.splice(cubes.indexOf(cubeToProcess), 1);
          cubes = cubes.concat(newCubes);
        }
      }

      while (cubes.some(x => cubes.some(y => y !== x && x.intersect(y)))) {
        var cubeToProcess = cubes.find(x => cubes.some(y => y !== x && x.intersect(y)));
        var cubeToAdd = cubes.find(x => x != cubeToProcess && cubeToProcess.intersect(x));
        var newCubes = cubeToProcess.addCube(cubeToAdd);
        cubes.splice(cubes.indexOf(cubeToAdd), 1);
        if (newCubes[0].containsFully(cubeToProcess)) {
          cubes.splice(cubes.indexOf(cubeToProcess), 1);
        }
        cubes = cubes.concat(newCubes);
      }
    });


    var count = 0;
    cubes.forEach(x => {
      console.log(x.toString());
      count += x.numberOfCubes;
    });

    // WRITE SOLUTION FOR TEST 1
    return count.toString();
  }

  public getSecondExpectedResult(): string {
    // RETURN EXPECTED SOLUTION FOR TEST 2;
    return 'day 1 solution 2';
  }
}
