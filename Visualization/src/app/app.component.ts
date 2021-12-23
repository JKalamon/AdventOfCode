import { Component, HostListener } from '@angular/core';

interface MapNode {
  selected: boolean;
  character: string;
  x: number;
  y: number;
}

export enum KEY_CODE {
  RIGHT_ARROW = 39,
  UP_ARROW = 38,
  DOWN_ARROW = 40,
  LEFT_ARROW = 37,
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'AOCVisualisation';
  mapInput: string = `#############
  #...........#
  ###A#C#B#A###
    #D#D#B#C#
    #########`;

  state = 1;

  constructor() {
    //this.goNext();
  }
  map: MapNode[][] = [];
  currentlySelected?: MapNode;
  energyUsed = 0;

  goNext() {
    this.energyUsed = 0;
    this.map = [];
    this.currentlySelected = undefined;
    var lines = this.mapInput.split('\n');
    for (let i = 0; i < lines.length; i++) {
      const mapLine: MapNode[] = [];
      this.map.push(mapLine);
      const line = lines[i];

      for (let j = 0; j < 13; j++) {
        mapLine.push({
          selected: false,
          character: line[j],
          x: j,
          y: i,
        });
      }
    }

    this.state = 2;
  }

  select(node: MapNode) {
    if (
      node.character == 'A' ||
      node.character == 'B' ||
      node.character == 'C' ||
      node.character == 'D'
    ) {
      if (this.currentlySelected) {
        this.currentlySelected.selected = false;
      }

      this.currentlySelected = node;
      node.selected = true;
    } else if (this.currentlySelected) {
      this.currentlySelected.selected = false;
      this.currentlySelected = undefined;
    }
  }

  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    console.log(event);

    if (event.key === 'ArrowUp') {
      this.moveUp();
    }

    if (event.key === 'ArrowDown') {
      this.moveDown();
    }

    if (event.key === 'ArrowLeft') {
      this.moveLeft();
    }

    if (event.key === 'ArrowRight') {
      this.moveRight();
    }
  }
  moveUp() {
    if (!this.currentlySelected) return;

    if (
      this.map[this.currentlySelected.y - 1][this.currentlySelected.x]
        ?.character === '.'
    ) {
      this.swapNodes(
        this.map[this.currentlySelected.y - 1][this.currentlySelected.x]
      );
    }
  }

  moveDown() {
    if (!this.currentlySelected) return;

    if (
      this.map[this.currentlySelected.y + 1][this.currentlySelected.x]
        ?.character === '.'
    ) {
      this.swapNodes(
        this.map[this.currentlySelected.y + 1][this.currentlySelected.x]
      );
    }
  }

  moveRight() {
    if (!this.currentlySelected) return;

    if (
      this.map[this.currentlySelected.y][this.currentlySelected.x + 1]
        ?.character === '.'
    ) {
      this.swapNodes(
        this.map[this.currentlySelected.y][this.currentlySelected.x + 1]
      );
    }
  }

  moveLeft() {
    if (!this.currentlySelected) return;

    if (
      this.map[this.currentlySelected.y][this.currentlySelected.x - 1]
        ?.character === '.'
    ) {
      this.swapNodes(
        this.map[this.currentlySelected.y][this.currentlySelected.x - 1]
      );
    }
  }

  private swapNodes(newNode: MapNode) {
    if (!this.currentlySelected) {
      return;
    }

    newNode.selected = true;
    newNode.character = this.currentlySelected.character;
    this.currentlySelected.character = '.';
    this.currentlySelected.selected = false;
    this.currentlySelected = newNode;
    this.calculateEnergy(newNode.character);
  }

  private calculateEnergy(character: string) {
    switch (character) {
      case 'A':
        this.energyUsed += 1;
        break;
      case 'B':
        this.energyUsed += 10;
        break;
      case 'C':
        this.energyUsed += 100;
        break;
      case 'D':
        this.energyUsed += 1000;
        break;
      default:
        break;
    }
  }
}
