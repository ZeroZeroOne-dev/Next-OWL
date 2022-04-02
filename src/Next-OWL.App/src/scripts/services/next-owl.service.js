import { NoGameError } from "../helpers/errors.js";

export class NextOwlService {

  // (static) class fields only work in chrome for now, revisit in future
  static get ApiBaseUrl() {
    return "https://next-owl.smets.dev/api";
  }

  async getNext() {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule/next`);
    return this.processResponse(r);
  }

  async getFuture(count = 4) {
    const r = await fetch(`${NextOwlService.ApiBaseUrl}/schedule?count=${count}`);
    return this.processResponse(r);
  }

  processResponse(response) {
    switch (response.status) {
      case 200:
        return response.json();
      case 204:
        throw new NoGameError();
      default:
        throw new Error('An error has occured');
    }
  }

}
