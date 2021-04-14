/* eslint-disable no-mixed-operators */
/* eslint-disable no-bitwise */
import { UTF8 } from './utf8';
export class Base64 {
    static encode(msg) {
        let t = '';
        let n;
        let r;
        let i;
        let s;
        let o;
        let u;
        let a;
        let f = 0;
        const e = msg;
        while (f < e.length) {
            n = e[f++];
            r = e[f++];
            i = e[f++];
            s = n >> 2;
            o = (n & 3) << 4 | r >> 4;
            u = (r & 15) << 2 | i >> 6;
            a = i & 63;
            if (Number.isNaN(r)) {
                u = 64;
                a = 64;
            }
            else if (Number.isNaN(i)) {
                a = 64;
            }
            t += Base64.keyStr.charAt(s);
            t += Base64.keyStr.charAt(o);
            t += Base64.keyStr.charAt(u);
            if (f < e.length)
                t += Base64.keyStr.charAt(a);
        }
        return t;
    }
    static decode(msg) {
        const t = [];
        let n;
        let r;
        let i;
        let s;
        let o;
        let u;
        let a;
        let f = 0;
        const e = msg.replace(/[^A-Za-z0-9+/=]/g, '');
        while (f < e.length) {
            s = this.keyStr.indexOf(e.charAt(f++));
            o = this.keyStr.indexOf(e.charAt(f++));
            u = this.keyStr.indexOf(e.charAt(f++));
            a = this.keyStr.indexOf(e.charAt(f++));
            n = s << 2 | o >> 4;
            r = (o & 15) << 4 | u >> 2;
            i = (u & 3) << 6 | a;
            t[f] = String.fromCharCode(n);
            if (u !== 64) {
                t[f] = String.fromCharCode(r);
            }
            if (a !== 64) {
                t[f] = String.fromCharCode(i);
            }
        }
        return UTF8.encode(t);
    }
    static escape(str) {
        return str.replace(/\+/g, '-')
            .replace(/\//g, '_')
            .replace(/=/g, '');
    }
    static urlEncode(input) {
        const encoded = Base64.encode(input);
        return Base64.escape(encoded);
    }
}
Base64.keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
//# sourceMappingURL=base64.js.map