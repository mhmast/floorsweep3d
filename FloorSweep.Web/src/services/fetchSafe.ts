import ConfigStore from '../store/configStore';
import TokenStore from '../store/tokenStore';
import { loginRedirect } from './authenticationService';

export interface Result<T>
{
    data:T,
    error:string,
    statusCode:number
}

export async function fetchSafe<T>(input:RequestInfo, init?:RequestInit)
: Promise<Result<T>> {
  try {
    const response = await fetch(input, init);
    if (response.ok) {
      const data = await response.json() as T;
      return { data, statusCode: response.status, error: null };
    }
    if (response.status === 401 || response.status === 403) {
      const configStore = ConfigStore.create();
      const config = await configStore.await();
      loginRedirect(config);
      return { data: null, statusCode: response.status, error: response.statusText };
    }
    return { data: null, statusCode: response.status, error: response.statusText };
  } catch (e) {
    return { data: null, statusCode: 500, error: e };
  }
}

export async function fetchAuthenticated<T>(input:RequestInfo, init?:RequestInit)
: Promise<Result<T>> {
  const tokenStore = TokenStore.create();
  const tokenData = await tokenStore.await();
  const newInit = init;
  newInit.headers = { ...init.headers, Authorization: `Bearer ${tokenData.token}` };
  return fetchSafe<T>(input, newInit);
}

export function formEncode(details:any):URLSearchParams {
  return new URLSearchParams(details);
}
