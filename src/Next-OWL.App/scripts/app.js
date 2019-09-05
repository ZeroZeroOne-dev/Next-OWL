import { NextOwlService } from "./service.js";

export class App {
  constructor(containerClass) {
    this.container = document.querySelector(`.${containerClass}`);
  }

  async run() {
    const service = new NextOwlService();
    this.nextGame = await service.getNextGame();
    this.draw();
  }

  getLocalDateString(UTCDateString) {
    const localDate = new Date(UTCDateString);

    const dateFormatOptions = {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric"
    };
    const dateFormat = Intl.DateTimeFormat("default", dateFormatOptions);

    const hourFormatOptions = {
      hour: "2-digit",
      minute: "2-digit"
    };
    const hourFormat = Intl.DateTimeFormat("default", hourFormatOptions);

    return `${dateFormat.format(localDate)} at ${hourFormat.format(localDate)}`;

    //shout-out to max
  }

  draw() {
    var localDate = this.getLocalDateString(this.nextGame.date);

    const template = `
        ${this.nextGame.teamOne} VS ${this.nextGame.teamTwo} on ${localDate}
    `;

    this.container.innerHTML = template;
  }
}
