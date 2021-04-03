import TokenStore from '../../store/tokenStore';
import { loginRedirect } from '../../services/authenticationService';
import ConfigStore from '../../store/configStore';
async function ensureToken() {
    const configStore = ConfigStore.create();
    const config = await configStore.await();
    return new Promise((resolve, reject) => {
        const tokenStore = TokenStore.create();
        const tokenData = tokenStore.get();
        if (!(tokenData === null || tokenData === void 0 ? void 0 : tokenData.token)) {
            reject();
            loginRedirect(config);
        }
        resolve();
    });
}
export const init = (() => ensureToken())();
//# sourceMappingURL=authenticated.js.map