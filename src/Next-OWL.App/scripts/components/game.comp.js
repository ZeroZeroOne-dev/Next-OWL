import { TeamComponent } from "./team.comp.js";
import { CountdownComponent } from "./countdown.comp.js";

export class GameComponent extends HTMLElement {
    constructor(game) {
        super();

        this.game = game;
        this.draw();
    }

    draw() {
        this.append(new TeamComponent(this.game.teamOne));
        this.append(new CountdownComponent(this.game.date));
        this.append(new TeamComponent(this.game.teamTwo));
    }
}
customElements.define('no-game', GameComponent);