import { DateHelper } from "../helpers/date-helper.js";

export class CountdownComponent extends HTMLElement {
    constructor(gameDate) {
        super();

        this.localDate = DateHelper.getLocalDateString(gameDate);
        this.gameDate = gameDate;

        this.draw();
        setInterval(() => {
            this.draw();
        }, 1000);
    }

    draw() {
        const template = `
            <span>${this.localDate}</span>
            <span>${DateHelper.getCountDownString(this.gameDate)}</span>
        `;

        this.innerHTML = template;
    }
}
customElements.define('no-countdown', CountdownComponent);