import * as querystring from 'query-string';
import { Base64 } from '../../services/base64';
import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
import { sha256 } from '../../services/cryptography';
import { tokenCallbackUrl } from '../routes';
import { UTF8 } from '../../services/utf8';

let parsedReturnUrl = '/';

if (typeof window !== 'undefined') {
  parsedReturnUrl = querystring.parse(window.location.search).returnUrl as string;
}

function getRandomString(length: number) {
  const randomChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
  let result = '';
  for (let i = 0; i < length; i++) {
    result += randomChars.charAt(Math.floor(Math.random() * randomChars.length));
  }
  return result;
}

export const init = (async () => {
  const configStore = ConfigStore.create();
  const tokenStore = TokenStore.create();
  const config = await configStore.await();
  const challenge = getRandomString(53);
  tokenStore.set({ challenge, token: null });
  const sha256val = sha256(challenge);

  const sha = Base64.urlEncode(sha256val);

  const returnUrl = `${config.baseUrl}${tokenCallbackUrl}?returnUrl=${encodeURIComponent(parsedReturnUrl)}`;
  const url = `${config.authentication.endpointConfiguration.authorization_endpoint}?response_type=code&client_id=${config.authentication.clientId}&redirect_uri=${returnUrl}&scope=openid&realm=${config.authentication.realm}&code_challenge=${sha}&code_challenge_method=S256`;
  // console.log({ url });
  if (typeof window !== 'undefined') {
    window.location.assign(url);
  }
});
