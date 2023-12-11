let greenCounter = 0;
let redCounter = 0;
let whiteCounter = 0;

fetch("/input.txt").then((x) => {
  x.text().then((maze) => {
    maze = maze.split("\r\n");
    const mazeWrapper = document.getElementById("maze");
    for (let y = 0; y < maze.length; y++) {
      const mazeLine = maze[y];
      for (let x = 0; x < mazeLine.length; x++) {
        const span = document.createElement("span");
        span.id = `x${x}y${y}`;
        whiteCounter++;
        span.innerHTML = document.getElementById(`node-${mazeLine[x]}`).outerHTML;
        span.classList.add("node");
        span.onclick = () => mark(x, y);
        mazeWrapper.appendChild(span);
      }

      mazeWrapper.appendChild(document.createElement("br"));
    }
  });

  fetch("/input.txt.json").then((x) => {
    x.json().then((loop) => {
      loop.forEach((part) => {
        const el = document.getElementById(`x${part.X}y${part.Y}`);
        el.classList.add("loop");
        whiteCounter--;
        redCounter++;
      });

      updateCounters();
    });
  });
});

function mark(x, y) {
  const el = document.getElementById(`x${x}y${y}`);
  if (!el) {
    return;
  }

  if (el.classList.contains("loop")) {
    return;
  }

  if (el.classList.contains("marked")) {
    el.classList.remove("marked");
    whiteCounter++;
    greenCounter--;
    subMark(x + 1, y, false);
    subMark(x - 1, y, false);
    subMark(x, y + 1, false);
    subMark(x, y - 1, false);
    subMark(x + 1, y + 1, false);
    subMark(x + 1, y - 1, false);
    subMark(x - 1, y + 1, false);
    subMark(x - 1, y - 1, false);
  } else {
    el.classList.add("marked");
    whiteCounter--;
    greenCounter++;
    subMark(x + 1, y, true);
    subMark(x - 1, y, true);
    subMark(x, y + 1, true);
    subMark(x, y - 1, true);
    subMark(x + 1, y + 1, true);
    subMark(x + 1, y - 1, true);
    subMark(x - 1, y + 1, true);
    subMark(x - 1, y - 1, true);
  }

  updateCounters();
}

function subMark(x, y, mark) {
  const el = document.getElementById(`x${x}y${y}`);
  if (!el) {
    return;
  }

  if (el.classList.contains("loop")) {
    return;
  }

  if (el.classList.contains("marked") && !mark) {
    el.classList.remove("marked");
    whiteCounter++;
    greenCounter--;
    subMark(x + 1, y, mark);
    subMark(x - 1, y, mark);
    subMark(x, y + 1, mark);
    subMark(x, y - 1, mark);
    subMark(x - 1, y - 1, mark);
    subMark(x - 1, y + 1, mark);
    subMark(x + 1, y + 1, mark);
    subMark(x + 1, y - 1, mark);
  } else if (!el.classList.contains("marked") && mark) {
    el.classList.add("marked");
    whiteCounter--;
    greenCounter++;
    subMark(x + 1, y, mark);
    subMark(x - 1, y, mark);
    subMark(x, y + 1, mark);
    subMark(x, y - 1, mark);
    subMark(x - 1, y - 1, mark);
    subMark(x - 1, y + 1, mark);
    subMark(x + 1, y + 1, mark);
    subMark(x + 1, y - 1, mark);
  }
}

function updateCounters() {
  document.getElementById("greenCounter").innerText = greenCounter;
  document.getElementById("redCounter").innerText = redCounter;
  document.getElementById("whiteCounter").innerText = whiteCounter;
}
