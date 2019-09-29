export class NextOwlService {

  //(static) class fields only work in chrome for now, revisit in future
  static get ApiBaseUrl() {
    return "https://next-owl.azurewebsites.net/api";
  }

  async getNextGame() {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule/nextgame`);

    return r.json();
  }

}
