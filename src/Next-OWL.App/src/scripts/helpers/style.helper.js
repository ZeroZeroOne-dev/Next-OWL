export class StyleHelper {

    static fadeOut(element) {
        return new Promise((resolve) => {
            element.style.opacity = 1;
            (function fade() {
                if ((element.style.opacity -= .1) < 0) {
                    element.style.display = "none"
                    resolve();
                } else {
                    setTimeout(fade, 40)
                }
            })();
        });
    }

}