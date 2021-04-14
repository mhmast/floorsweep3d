/* eslint-disable no-mixed-operators */
/* eslint-disable no-bitwise */
export class UTF8 {
    static encode(msg) {
        const e = msg;
        let t = '';
        for (let n = 0; n < e.length; n++) {
            const r = e[n];
            if (r < 128) {
                t += String.fromCharCode(r);
            }
            else if (r > 127 && r < 2048) {
                t += String.fromCharCode(r >> 6 | 192);
                t += String.fromCharCode(r & 63 | 128);
            }
            else {
                t += String.fromCharCode(r >> 12 | 224);
                t += String.fromCharCode(r >> 6 & 63 | 128);
                t += String.fromCharCode(r & 63 | 128);
            }
        }
        return t;
    }
    static decode(e) {
        const t = [];
        let n = 0;
        let r = 0;
        let c2 = 0;
        while (n < e.length) {
            r = e.charCodeAt(n);
            if (r < 128) {
                t[n] = String.fromCharCode(r);
                n++;
            }
            else if (r > 191 && r < 224) {
                c2 = e.charCodeAt(n + 1);
                t[n] = String.fromCharCode((r & 31) << 6 | c2 & 63);
                n += 2;
            }
            else {
                c2 = e.charCodeAt(n + 1);
                const c3 = e.charCodeAt(n + 2);
                t[n] = String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63);
                n += 3;
            }
        }
        return t;
    }
}
//# sourceMappingURL=utf8.js.map