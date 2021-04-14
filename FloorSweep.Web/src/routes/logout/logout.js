import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
import { home } from '../routes';
export function redirect(config, returnUrl) {
    const tokenStore = TokenStore.create();
    tokenStore.set({ challenge: null, token: null });
    const url = `${config.authentication.endpointConfiguration.end_session_endpoint}`;
    if (typeof window !== 'undefined') {
        window.location.assign(`${url}?post_logout_redirect_uri=${encodeURIComponent(returnUrl)}`);
    }
}
export async function init() {
    const configStore = ConfigStore.create();
    const config = await configStore.await();
    redirect(config, `${config.baseUrl}${home}`);
}
//# sourceMappingURL=logout.js.map