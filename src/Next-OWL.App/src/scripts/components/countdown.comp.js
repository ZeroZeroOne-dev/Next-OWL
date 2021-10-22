import { DateHelper } from "../helpers/date.helper.js";
import { Events } from "../helpers/events.js";

export class CountdownComponent extends HTMLElement {
    constructor(gameDate) {
        super();

        this.localDate = DateHelper.getLocalDateString(gameDate);
        this.gameDate = gameDate;

        this.countDown();
        this.drawInterval = setInterval(() => {
            this.countDown();
        }, 500);
    }

    countDown() {
        const timeToGame = DateHelper.getSpan(this.gameDate);

        if (timeToGame > 0) {
            this.draw(timeToGame);
            return
        }

        clearInterval(this.drawInterval);
        this.draw(0);

        setTimeout(() => {
            this.dispatchEvent(new Event(Events.GameStarted, { bubbles: true }));
        }, 2000);
    }

    draw(timeToGame) {
        const template = `
            <span>${this.localDate}</span>
            <span>${DateHelper.getCountDownString(timeToGame)}</span>
        `;

        this.innerHTML = template;
    }
}
customElements.define('no-countdown', CountdownComponent);