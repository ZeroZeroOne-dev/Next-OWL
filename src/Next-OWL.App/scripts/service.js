export class NextOwlService {
  static ApiBaseUrl = "https://next-owl.azurewebsites.net/api";

  async getNextGame() {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule/nextgame`);

    return r.json();
  }

  async getFakeNextGame() {
    return {
      teamOne: {
        name: "Seoul Dynasty",
        icon: "https://bnetcmsus-a.akamaihd.net/cms/page_media/E9MU0AK0JIXT1507858876249.svg"
      },
      teamTwo: {
        name: "Vancouver Titans",
        icon: "https://bnetcmsus-a.akamaihd.net/cms/gallery/0KOSPFU6UC411543976755522.svg"
      },
      date: "2019-09-05T23:00:00Z"
    };
  }
}
