export class NoGameError extends Error {
    constructor() {
        const text = `
            The Overwatch League has not yet released their new schedule, check again later. <br/>
        `;

        super(text);
    }
};