export class ErrorComponent extends HTMLElement {

    constructor(error) {
        super();

        this.error = error;
        this.draw();
    }

    draw() {
        this.innerHTML = `
            ${this.error.message} <br/>
            Double click to refresh.
        `
    }

}
customElements.define('no-error', ErrorComponent);