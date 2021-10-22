export class TeamComponent extends HTMLElement {
    constructor(team) {
        super();

        this.team = team;
        this.draw();
    }

    draw() {
        const template = `
            ${this.team.name}
            <img src="${this.team.icon}"/>
        `;

        this.innerHTML = template;
    }
}
customElements.define('no-team', TeamComponent);