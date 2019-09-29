export class SpinnerComponent extends HTMLElement {

    constructor() {
        super();

        this.draw();
    }

    draw() {
        this.innerHTML = '<img src="./images/oval.svg"/>'
    }

}
customElements.define('no-spinner', SpinnerComponent);