import { NextOwlService } from "./service.js";

export function run() {
  const service = new NextOwlService();
  service.nextGame().then(console.log);
}
