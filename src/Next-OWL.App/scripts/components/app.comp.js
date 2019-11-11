import { NextOwlService } from "../services/service.js";
import { TeamComponent } from "./team.comp.js";
import { CountdownComponent } from "./countdown.comp.js";
import { SpinnerComponent } from "./spinner.comp.js";
import { ErrorComponent } from "./error.component.js";

export class AppComponent extends HTMLElement {
  constructor() {
    super();

    this.nextOwlService = new NextOwlService();

    this.run();
    this.addEventListener('dblclick', () => this.run());
  }

  run() {
    this.drawLoading();
    this.nextOwlService.getNextGame().then(nextGame => {
      this.nextGame = nextGame;
      this.draw();
    }).catch((er) => {
      this.drawError(er);
    })
  }

  drawLoading() {
    this.replaceAll(new SpinnerComponent());
  }

  draw() {
    const container = document.createElement('div');
    container.classList.add('teams');

    container.append(new TeamComponent(this.nextGame.teamOne));
    container.append(new CountdownComponent(this.nextGame.date));
    container.append(new TeamComponent(this.nextGame.teamTwo));

    this.replaceAll(container);
  }

  drawError(error) {
    const container = document.createElement('div');

    container.append(new ErrorComponent(error));

    this.replaceAll(container);
  }

  replaceAll(newContent) {
    while (this.firstChild) {
      this.removeChild(this.firstChild);
    }

    this.append(newContent);
  }
}
customElements.define('no-app', AppComponent);