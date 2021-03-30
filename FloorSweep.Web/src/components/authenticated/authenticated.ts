import TokenStore from '../../store/tokenStore';
import { loginRedirect } from '../../services/authenticationService';

async function ensureToken() {
  const tokenStore = TokenStore.create();
  const tokenData = tokenStore.get();
  if (!tokenData?.token) {
    loginRedirect();
  }
}

export const init = (() => ensureToken())();
