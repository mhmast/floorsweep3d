import type { Configuration } from '../../models/configuration';
import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
import { home } from '../routes';

export function redirect(config:Configuration, returnUrl:string) {
  const tokenStore = TokenStore.create();
  tokenStore.set({ challenge: null, token: null });
  const url = `${config.authentication.endpointConfiguration.end_session_endpoint}`;
  if (typeof window !== 'undefined') {
    window.location.assign(`${url}?post_logout_redirect_uri=${encodeURIComponent(returnUrl)}`);
  }
}
export async function init() : Promise<void> {
  const configStore = ConfigStore.create();
  const config = await configStore.await();
  redirect(config, `${config.baseUrl}${home}`);
}
