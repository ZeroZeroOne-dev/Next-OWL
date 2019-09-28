import { NextOwlService } from "../services/service.js";
import { TeamComponent } from "./team.component.js";
import { CountdownComponent } from "./countdown.component.js";

export class AppComponent extends HTMLElement {
  constructor() {
    super();

    this.nextOwlService = new NextOwlService();

    this.run();
  }

  run() {
    this.nextOwlService.getNextGame().then(nextGame => {
      this.nextGame = nextGame;
      this.draw();
    });
  }

  draw() {
    const container = document.createElement('div');
    container.classList.add('teams');

    container.append(new TeamComponent(this.nextGame.teamOne));
    container.append(new CountdownComponent(this.nextGame.date));
    container.append(new TeamComponent(this.nextGame.teamTwo));

    this.append(container);
  }
}
customElements.define('no-app', AppComponent);