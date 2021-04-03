import * as querystring from 'query-string';
import ConfigStore from '../../store/configStore';
import TokenStore from '../../store/tokenStore';
import { tokenCallbackUrl } from '../routes';
import { fetchSafe, formEncode } from '../../services/fetchSafe';
let parsedCode = '';
let parsedReturnUrl = '';
if (typeof window !== 'undefined') {
    const parsed = querystring.parse(window.location.search);
    parsedCode = parsed.code;
    parsedReturnUrl = parsed.returnUrl;
}
export const init = (async () => {
    const configStore = ConfigStore.create();
    const tokenStore = TokenStore.create();
    const config = await configStore.await();
    const tokenData = await tokenStore.await();
    const formData = {
        code_verifier: tokenData.challenge,
        code: parsedCode,
        redirect_uri: `${config.baseUrl}${tokenCallbackUrl}?returnUrl=${parsedReturnUrl}`,
        client_secret: config.authentication.clientSecret,
        client_id: config.authentication.clientId,
        grant_type: 'authorization_code',
    };
    const url = config.authentication.endpointConfiguration.token_endpoint;
    const tokenObject = await fetchSafe(url, { body: formEncode(formData), method: 'POST' });
    if (tokenObject.error) {
        throw tokenObject.error;
    }
    tokenData.token = tokenObject.data.access_token;
    tokenStore.set(tokenData);
    if (typeof window !== 'undefined') {
        window.location.assign(parsedReturnUrl);
    }
});
//# sourceMappingURL=token.js.map