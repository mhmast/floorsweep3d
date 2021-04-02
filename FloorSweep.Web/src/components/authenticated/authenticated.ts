import TokenStore from '../../store/tokenStore';
import { loginRedirect } from '../../services/authenticationService';
import ConfigStore from '../../store/configStore';

async function ensureToken(): Promise<void> {
  const configStore = ConfigStore.create();
  const config = await configStore.await();
  return new Promise<void>((resolve, reject) => {
    const tokenStore = TokenStore.create();
    const tokenData = tokenStore.get();
    if (!tokenData?.token) {
      reject();
      loginRedirect(config);
    }
    resolve();
  });
}

export const init = (() => ensureToken())();
