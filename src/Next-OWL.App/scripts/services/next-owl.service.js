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
      default:
        throw new Error('An error has occured');
    }
  }

  async getFuture(count = 4) {
    const request = await fetch(`${NextOwlService.ApiBaseUrl}/schedule?count=${count}`);

    switch (request.status) {
      case 200:
        return await this.CheckData(request);
      default:
        throw new Error('An error has occured');
    }
  }

  async CheckData(request) {
    var data = await request.json();
    if (data.length > 0) {
      return data;
    } else {
      throw new NoGameError();
    }
  }

}
