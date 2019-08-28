export class NextOwlService {
  static ApiBaseUrl = "https://next-owl.azurewebsites.net/api";

  nextGame() {
    fetch(`${NextOwlService.ApiBaseUrl}/schedule/nextgame`, {
      mode: "no-cors"
    }).then(r => console.log(r));
  }
}
