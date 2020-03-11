export class NoGameError extends Error {
    constructor() {
        super('No game found.');
    }
};

export class CoronaError extends Error {
    constructor() {
        const text = `
            The March and April Overwatch league events have been canceled.
            <a href="https://twitter.com/overwatchleague/status/1237824370963144704">Official statement</a>
        `;

        super(text);
    }
}