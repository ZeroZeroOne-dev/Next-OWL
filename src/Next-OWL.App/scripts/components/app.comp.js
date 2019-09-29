import { NextOwlService } from "../services/service.js";
import { TeamComponent } from "./team.comp.js";
import { CountdownComponent } from "./countdown.comp.js";
import { SpinnerComponent } from "./spinner.comp.js";

export class AppComponent extends HTMLElement {
  constructor() {
    super();

    this.nextOwlService = new NextOwlService();

    this.run();
  }

  run() {
    this.drawLoading();
    this.nextOwlService.getNextGame().then(nextGame => {
      this.nextGame = nextGame;
      this.draw();
    });
  }

  drawLoading() {
    this.append(new SpinnerComponent());
  }

  draw() {
    const container = document.createElement('div');
    container.classList.add('teams');

    container.append(new TeamComponent(this.nextGame.teamOne));
    container.append(new CountdownComponent(this.nextGame.date));
    container.append(new TeamComponent(this.nextGame.teamTwo));

    this.removeChild(this.children[0]);
    this.append(container);
  }
}
customElements.define('no-app', AppComponent);