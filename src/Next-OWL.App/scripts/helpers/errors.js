export class NoGameError extends Error {
    constructor() {
        super('No game found.');
    }
};