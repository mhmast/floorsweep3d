import {loginUrl as baseUrl} from "../routes/routes";

export function loginRedirect()
{
    if(typeof window !== "undefined")
    {
        const currentLocation = window.location.toString();
        const loginUrl = `${baseUrl}?returnUrl=${encodeURIComponent(currentLocation)}`;
        window.location.assign(loginUrl);
    }
}