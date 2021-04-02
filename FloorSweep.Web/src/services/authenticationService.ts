import { loginUrl as loginPath } from '../routes/routes';
import { redirect } from '../routes/logout/logout';
import type { Configuration } from '../models/configuration';

export function loginRedirect(config:Configuration) {
  if (typeof window !== 'undefined') {
    const currentLocation = window.location.toString();
    const loginUrl = `${config.baseUrl}${loginPath}?returnUrl=${encodeURIComponent(currentLocation)}`;
    redirect(config, loginUrl);
  }
}
