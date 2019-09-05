import { NextOwlService } from "./service.js";

export class App {
  constructor(containerClass) {
    this.container = document.querySelector(`.${containerClass}`);
    this.owlService = new NextOwlService();
  }

  async run() {
    this.nextGame = await this.owlService.getFakeNextGame();
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
      <div class="next-result">
        <div class="teams">
          <div class="team left">${this.nextGame.teamOne.name}</div>
          <div class="team right">${this.nextGame.teamTwo.name}</div>
        </div>
        <div class="when">${localDate}</div>
      </div>
    `;

    this.container.innerHTML = template;
  }
}
