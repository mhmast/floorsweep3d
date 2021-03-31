import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
import { home } from '../routes';

export const init = (async () => {
  const configStore = ConfigStore.create();
  const tokenStore = TokenStore.create();
  const config = await configStore.await();
  tokenStore.set({ challenge: null, token: null });
  const url = `${config.authentication.endpointConfiguration.end_session_endpoint}`;
  const redirectUrl = encodeURIComponent(`${config.baseUrl}${home}`);
  if (typeof window !== 'undefined') {
    window.location.assign(`${url}?post_logout_redirect_uri=${redirectUrl}`);
  }
});
