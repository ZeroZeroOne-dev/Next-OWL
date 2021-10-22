import { DateHelper } from "../helpers/date.helper.js";

export class TitleHelper {
    static setGame(nextGame) {
        clearInterval(TitleHelper.setTitleInterval);

        TitleHelper.setTitleInterval = setInterval(() => TitleHelper.setTitle(nextGame), 1000);
    }

    // private static methods are still an experimental feature: #setTitle -> # marks it as private
    static setTitle(nextGame) {
        const timeToGame = DateHelper.getSpan(nextGame.date);
        const countDown = DateHelper.getCountDownString(timeToGame);
        const teamOneShort = nextGame.teamOne.shortName;
        const teamTwoShort = nextGame.teamTwo.shortName;

        const title = `${countDown} | ${teamOneShort} vs ${teamTwoShort}`;

        document.title = title;
    }
}