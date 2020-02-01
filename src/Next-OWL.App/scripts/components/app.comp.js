import { NextOwlService } from "../services/next-owl.service.js";
import { TitleHelper } from "../helpers/title.helper.js";
import { SpinnerComponent } from "./spinner.comp.js";
import { ErrorComponent } from "./error.component.js";
import { GameComponent } from "./game.comp.js";

export class AppComponent extends HTMLElement {
  constructor() {
    super();

    this.nextOwlService = new NextOwlService();

    this.run();
    this.addEventListener('dblclick', () => this.run());
  }

  run() {
    this.drawLoading();
    this.nextOwlService.getFuture(4)
      .then(nextGames => {
        TitleHelper.setGame(nextGames[0]);
        this.nextGames = nextGames;
        this.draw();
      })
      .catch((er) => {
        this.drawError(er);
        throw er;
      });
  }

  drawLoading() {
    this.clearAll();
    this.appendChild(new SpinnerComponent());
  }

  draw() {
    this.clearAll();

    for (var i = 0; i < this.nextGames.length; i++) {
      const game = this.nextGames[i];
      const gameComponent = new GameComponent(game);

      if (i != 0) {
        gameComponent.classList.add('not-first-game');
      }

      this.append(gameComponent);
      this.append(document.createElement('hr'));
    }
  }

  drawError(error) {
    this.clearAll();
    this.append(new ErrorComponent(error));
  }

  clearAll() {
    while (this.firstChild) {
      this.removeChild(this.firstChild);
    }
  }
}
customElements.define('no-app', AppComponent);