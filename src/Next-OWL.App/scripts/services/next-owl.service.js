import { NoGameError } from "../helpers/errors.js";

export class NextOwlService {

  // (static) class fields only work in chrome for now, revisit in future
  static get ApiBaseUrl() {
    return "https://next-owl.azurewebsites.net/api";
  }

  async getNext() {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule/next`);

    switch (r.status) {
      case 200:
        return r.json();
      case 204:
        throw new NoGameError();
      default:
        throw new Error('An error has occured');
    }
  }

  async getFuture(count = 4) {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule?count=${count}`);

    switch (r.status) {
      case 200:
        return r.json();
      case 204:
        throw new NoGameError();
      default:
        throw new Error('An error has occured');
    }
  }

}
