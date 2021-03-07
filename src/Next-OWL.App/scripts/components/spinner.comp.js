export class SpinnerComponent extends HTMLElement {

    constructor() {
        super();

        this.draw();
    }

    draw() {
        this.innerHTML = '<div class="loader"></div>'
    }

}
customElements.define('no-spinner', SpinnerComponent);