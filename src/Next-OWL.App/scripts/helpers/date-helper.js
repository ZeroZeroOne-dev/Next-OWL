export class DateHelper {
    static getLocalDateString(UTCDateString) {
        const localDate = new Date(UTCDateString);

        const dateFormatOptions = {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric"
        };
        const dateFormat = Intl.DateTimeFormat("default", dateFormatOptions);

        const hourFormatOptions = {
            hour: "2-digit",
            minute: "2-digit"
        };
        const hourFormat = Intl.DateTimeFormat("default", hourFormatOptions);

        return `${dateFormat.format(localDate)} at ${hourFormat.format(localDate)}`;

        //shout-out to max
    }
}