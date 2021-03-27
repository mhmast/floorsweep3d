import queryString from "query-string";
import base64url from "base64url";
import { Sha256 } from "../../services/cryptography"
import { set as setTokenData } from "../../store/tokenData";
import { get as getConfig } from "../../store/config";
import { tokenCallbackUrl } from "../routes"

let parsedReturnUrl = "/";

if (typeof window !== 'undefined') {
  parsedReturnUrl = queryString.parse(window.location.search)["returnUrl"] as string;
}

export let init = (async () => {
  const challenge = getRandomString(53);
  setTokenData({ challenge, token: null });
  const sha256 = Sha256.hash(challenge) as string;
  var sha = base64url(sha256);
  const config = getConfig();
  const returnUrl = `${config.baseUrl}${tokenCallbackUrl}?returnUrl=${encodeURIComponent(parsedReturnUrl)}`;
  var url = `${config.authentication.endpointConfiguration.authorization_endpoint}?response_type=code&client_id=${config.authentication.clientId}&redirect_uri=${returnUrl}&scope=openid&realm=${config.authentication.realm}&code_challenge=${sha}&code_challenge_method=S256`;
  return url;
})();

function getRandomString(length) {
  var randomChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
  var result = '';
  for (var i = 0; i < length; i++) {
    result += randomChars.charAt(Math.floor(Math.random() * randomChars.length));
  }
  return result;
}