import { NextOwlService } from "../services/service.js";
import { DateHelper } from "../helpers/date-helper.js";
import { TeamComponent } from "./team.component.js";

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
    const teamOneElement = new TeamComponent(this.nextGame.teamOne);

    this.append(teamOneElement);
  }
}
customElements.define('no-app', AppComponent);