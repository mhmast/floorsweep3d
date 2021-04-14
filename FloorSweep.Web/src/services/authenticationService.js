import { loginUrl as loginPath } from '../routes/routes';
import { redirect } from '../routes/logout/logout';
export function loginRedirect(config) {
    if (typeof window !== 'undefined') {
        const currentLocation = window.location.toString();
        const loginUrl = `${config.baseUrl}${loginPath}?returnUrl=${encodeURIComponent(currentLocation)}`;
        redirect(config, loginUrl);
    }
}
//# sourceMappingURL=authenticationService.js.map