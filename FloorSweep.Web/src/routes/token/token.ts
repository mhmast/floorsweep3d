import * as querystring from 'query-string';
import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';

import fetchSafe, { formEncode } from '../../services/fetchSafe';

let parsedCode = '';
let parsedReturnUrl = '';

if (typeof window !== 'undefined') {
  const parsed = querystring.parse(window.location.search);
  parsedCode = parsed.code as string;
  console.log(parsedCode);
  parsedReturnUrl = parsed.returnUrl as string;
}
interface TokenResponse{
  code:string;
}
export const init = (async () => {
  const configStore = ConfigStore.create();
  const tokenStore = TokenStore.create();
  const config = await configStore.await();
  const tokenData = await tokenStore.await();

  const formData = {
    code_verifier: tokenData.challenge,
    code: parsedCode,
    // redirect_uri","https://localhost/MonitorToken.html"},
    client_secret: config.authentication.clientSecret,
    client_id: config.authentication.clientId,
    grant_type: 'authorization_code',
  };

  const url = config.authentication.endpointConfiguration.token_endpoint;
  const tokenObject = await fetchSafe<TokenResponse>(url, { body: formEncode(formData), method: 'POST' });
  if (tokenObject.error) {
    throw tokenObject.error;
  }
  tokenData.token = tokenObject.data.code;
  tokenStore.set(tokenData);
  if (typeof window !== 'undefined') {
    window.location.assign(parsedReturnUrl);
  }
});
