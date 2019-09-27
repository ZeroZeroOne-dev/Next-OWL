export class TeamComponent extends HTMLElement {
    constructor(team) {
        super();

        this.team = team;
        this.draw();
    }

    draw() {
        const template = `
            ${this.team.name}
        `;

        this.innerHTML = template;
    }
}
customElements.define('no-team', TeamComponent);