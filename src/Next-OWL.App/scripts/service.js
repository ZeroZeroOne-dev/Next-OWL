export class NextOwlService {
  static ApiBaseUrl = "https://next-owl.azurewebsites.net/api";

  async getNextGame() {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule/nextgame`);

    return r.json();
  }
}
