import ConfigStore from '../store/configStore';
import TokenStore from '../store/tokenStore';
import { loginRedirect } from './authenticationService';
export async function fetchSafe(input, init) {
    try {
        const response = await fetch(input, init);
        if (response.ok) {
            const data = await response.json();
            return { data, statusCode: response.status, error: null };
        }
        if (response.status === 401 || response.status === 403) {
            const configStore = ConfigStore.create();
            const config = await configStore.await();
            loginRedirect(config);
            return { data: null, statusCode: response.status, error: response.statusText };
        }
        return { data: null, statusCode: response.status, error: response.statusText };
    }
    catch (e) {
        return { data: null, statusCode: 500, error: e };
    }
}
export async function fetchAuthenticated(input, init) {
    const tokenStore = TokenStore.create();
    const tokenData = await tokenStore.await();
    const newInit = init;
    newInit.headers = Object.assign(Object.assign({}, init.headers), { Authorization: `Bearer ${tokenData.token}` });
    return fetchSafe(input, newInit);
}
export function formEncode(details) {
    return new URLSearchParams(details);
}
//# sourceMappingURL=fetchSafe.js.map